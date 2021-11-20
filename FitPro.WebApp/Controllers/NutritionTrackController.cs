using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitPro.BusinessLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitPro.WebApp.Controllers
{
    public class NutritionTrackController : BaseController
    {
        private readonly NutritionTrackService Service;
        public NutritionTrackController(ControllerDependencies dependencies, NutritionTrackService service) : base(dependencies)
        {
            this.Service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Policy = "RegularOnly")]
        public IActionResult DailyTrack(DateTime date, Guid idRegularUser)
        {
            var model = Service.GetDailyList(date, idRegularUser);
            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "RegularOnly")]
        public IActionResult AddAlimentTrack()
        {
            var model = new SaveAlimentTrackModel();
            model.Date = DateTime.Now;

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "RegularOnly")]
        public IActionResult AddAlimentTrack(SaveAlimentTrackModel model, Guid idRegularUser)
        {
            model.IdRegularUser = idRegularUser;
            Service.AddAlimentTrack(model);
            return RedirectToAction(
                "DailyTrack", 
                "NutritionTrack", 
                new { date = model.Date, idRegularUser = idRegularUser }
            );
        }

        [HttpGet]
        [Authorize(Policy = "RegularOnly")]
        public IActionResult EditAlimentTrack(Guid idAliment,
            Guid idRegularUser, DateTime date)
        {
            var model = Service.GetEditAlimentTrackModel(idAliment, idRegularUser, date);
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "RegularOnly")]
        public IActionResult EditAlimentTrack(SaveAlimentTrackModel model, Guid idRegularUser)
        {
            model.IdRegularUser = idRegularUser;
            Service.EditAlimentTrack(model);
            return RedirectToAction(
                "DailyTrack",
                "NutritionTrack",
                new { date = model.Date, idRegularUser = idRegularUser }
            );
        }

        [HttpPost]
        [Authorize(Policy = "RegularOnly")]
        public IActionResult DeleteAlimentTrack(Guid idAliment,
            Guid idRegularUser, DateTime date)
        {
            Service.DeleteAlimentTrack(idAliment, idRegularUser, date);  
            return RedirectToAction(
                "DailyTrack",
                "NutritionTrack",
                new { date = date, idRegularUser = idRegularUser }
            );
        }

    }
}
