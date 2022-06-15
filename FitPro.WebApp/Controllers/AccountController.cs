using FitPro.BusinessLogic;
using FitPro.Common.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FitPro.WebApp.Controllers
{
    public class AccountController : BaseController
    {
        private readonly RegularUserAccountService Service;

        public AccountController(ControllerDependencies dependencies, RegularUserAccountService service) : base(dependencies)
        {
            this.Service = service;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var currentUser = Service.Login(model);

            if(!currentUser.IsAuthenticated)
            {
                return View(model);
            }

            await LogIn(currentUser);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegularRegisterModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Register(RegularRegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Service.RegisterRegularUser(model);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await LogOut();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult EditRegularProfile()
        {
            var model = Service.ProfileRegularUserById(CurrentUser.Id);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditRegularProfile(RegularProfileModel model)
        {

            Service.EditProfile(model);
            model.Password = "";

            return View(model);
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            var newModel = Service.GetChangePasswordModel(CurrentUser.Id);
            return View(newModel);
        }

        [HttpPost]
        public async Task<ActionResult> ChangePasswordAsync(ChangePasswordModel model)
        {
            Service.ChangePassword(model);

            await LogOut();

            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult DeleteAccount()
        {
            var newModel = Service.GetLoginModel(CurrentUser.Id);
            return View(newModel);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteAccount(LoginModel model)
        {
            Service.DeleteAccount(model);

            await LogOut();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult EditSpecialProfile()
        {
            var model = Service.ProfileSpecialUserById(CurrentUser.Id);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditSpecialProfile(SpecialProfileModel model)
        {
            Service.EditProfileSpecial(model);
            return View(model);
        }

        [HttpGet]
        public ActionResult ProfilePicByUserName(string userName)
        {
            var image = Service.GetProfilePicOrDefault(userName);
            return File(image, "image/jpeg");
        }


        [HttpGet]
        public IActionResult Details(string userName)
        {
            var role = Service.GetRoleByUserName(userName);
            switch (role)
            {
                case "Regular":
                    return RedirectToAction("DetailsRegular", "Account", new { userName = userName});
                case "Nutritionist":
                    return RedirectToAction("DetailsNutritionist", "Account", new { userName = userName });
                case "Trainer":
                    return RedirectToAction("DetailsTrainer", "Account", new { userName = userName });
                case "Admin":
                    return RedirectToAction("DetailsAdmin", "Account", new { userName = userName });
                default:
                    return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult DetailsRegular(string userName)
        {
            var model = Service.GetDetailsRegular(userName);
            return View(model);
        }

        [HttpGet]
        public IActionResult DetailsNutritionist(string userName)
        {
            var model = Service.GetDetailsNutritionist(userName);
            return View(model);
        }

        [HttpGet]
        public IActionResult DetailsTrainer(string userName)
        {
            var model = Service.GetDetailsTrainer(userName);
            return View(model);
        }

        [HttpGet]
        public IActionResult DetailsAdmin(string userName)
        {
            var model = Service.GetDetailsAdmin(userName);
            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            var model = new ForgotPasswordModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordModel model)
        {
            var token = Service.ForgotPassword(model);
            var link = Url.Action("ResetForgotPassword", "Account",
                new
                {
                    email = model.Email,
                    code = token
                }, "http");

            var lnkHref = "<a href =\"" + link + "\" > Click here </a > ";
            
            EmailManager.SendEmail(lnkHref, model.Email);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ResetForgotPassword(string email)
        {
            var model = Service.ResetForgotPassword(email);
            return View(model);
        }

        private async Task LogIn(CurrentUserDto user)
        {
            var claims = new List<Claim>
            {
                new Claim("Id", user.Id.ToString()),
                new Claim("UserName", user.UserName),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim("Role", user.Role),
                new Claim("Streak", user.Streak.ToString())
            };

            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                    scheme: "FitProCookies",
                    principal: principal);
        }


        private async Task LogOut()
        {
            await HttpContext.SignOutAsync(scheme: "FitProCookies");
        }
    }
}
