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
        public IActionResult AdminAddUser(Guid idAdmin)
        {
            var model = new AdminAddUserModel();
            Service.PopulateRolesDropDown(model);
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult AdminAddUser(AdminAddUserModel model)
        {
            Service.PopulateRolesDropDown(model);
            Service.AdminAddUser(model);
            return RedirectToAction("UsersList", "Admin", new { currentPage = 1, rowNumber = 10 });
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult AdminDeleteUser(Guid idAdmin, string emailUser)
        {
            var model = Service.GetDeleteUserModel(idAdmin, emailUser);
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
