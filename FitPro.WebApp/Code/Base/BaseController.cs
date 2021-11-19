using Microsoft.AspNetCore.Mvc;
using FitPro.Common.DTOs;
using Microsoft.AspNetCore.Mvc.Filters;
using FitPro.Common;
using System;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace FitPro.WebApp
{
    public class BaseController : Controller
    {
        protected readonly CurrentUserDto CurrentUser;

        public BaseController(ControllerDependencies dependencies)
            : base()
        {
            CurrentUser = dependencies.CurrentUser;
        }

    }
}
