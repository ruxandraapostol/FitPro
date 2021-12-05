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
        public IActionResult DailyTrack(Guid idRegularUser, DateTime? date = null)
        {
            if(date == null)
            {
                date = DateTime.Now;
            }
            
            var model = Service.GetDailyList(date.Value, idRegularUser);
            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "RegularOnly")]
        public IActionResult AddAlimentTrack(DateTime? date)
        {
            var model = new SaveAlimentTrackModel();
            if (date == null)
            {
                model.Date = DateTime.Now;
            }
            else
            {
                model.Date = date.Value;
            }

            model.FoodList = Service.PopulateFoodList();

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "RegularOnly")]
        public IActionResult AddAlimentTrack(SaveAlimentTrackModel model, Guid idRegularUser)
        {
            model.IdRegularUser = idRegularUser;
            model.FoodList = Service.PopulateFoodList();
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
