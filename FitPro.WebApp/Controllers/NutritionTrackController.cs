using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FitPro.WebApp.Controllers
{
    public class NutritionTrackController : BaseController
    {
        private readonly NutritionTrackService Service
        public NutritionTrackController(ControllerDependencies dependencies, NutritionTrackService service) : base(dependencies)
        {
            this.Service = service;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
