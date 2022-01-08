using FitPro.BusinessLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FitPro.WebApp.Controllers
{
    public class AdminController : BaseController
    {
        private readonly AdminService Service;

        public AdminController(ControllerDependencies dependencies, AdminService service) : base(dependencies)
        {
            this.Service = service;
        }

        [Authorize(Policy = "AdminOnly")]
        public IActionResult AdminUsersList(int currentPage, int rowNumber, string sortColumn, int sortColumnInndex, string searchString)
        {
            var model = Service.GetUsers(currentPage, rowNumber, sortColumn, sortColumnInndex, searchString);
            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult AdminAddUser()
        {
            var model = new AdminAddUserModel();
            Service.PopulateRolesDropDown(model);
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult AdminAddUser(AdminAddUserModel model)
        {
            model.IdAdmin = CurrentUser.Id;
            Service.PopulateRolesDropDown(model);
            Service.AdminAddUser(model);
            return RedirectToAction("AdminUsersList", "Admin", new { currentPage = 1, rowNumber = 10 });
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult AdminDeleteUser(string emailUser)
        {
            var model = Service.GetDeleteUserModel(CurrentUser.Id, emailUser);
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult AdminDeleteUser(AdminDeleteUserModel model)
        {
            Service.AdminDeleteUser(model);
            return RedirectToAction("UsersList", "Admin", new { currentPage = 1, rowNumber = 10});
        }

    }
}
