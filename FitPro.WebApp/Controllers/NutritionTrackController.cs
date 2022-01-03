using System;
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
        public IActionResult DailyTrack(Guid idRegularUser, string date = "")
        {
            var actualDate = string.IsNullOrEmpty(date) ?
                    DateTime.Now : DateTime.Parse(date);
            
            var model = Service.GetDailyList(actualDate, idRegularUser);
            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "RegularOnly")]
        public IActionResult NavigateDay(Guid idRegularUser, bool prev, string date = "")
        {
            var actualDate = string.IsNullOrEmpty(date) ?
                    DateTime.Now : DateTime.Parse(date);

            var prevDay = prev? actualDate.AddDays(-1) : actualDate.AddDays(1);

            return RedirectToAction(
                "DailyTrack",
                "NutritionTrack",
                new { date = prevDay.ToString("dd/MM/yyyy HH:mm"), idRegularUser = idRegularUser }
            );
        }

        [HttpGet]
        [Authorize(Policy = "RegularOnly")]
        public IActionResult AddAlimentTrack(string date)
        {
            var model = new SaveAlimentTrackModel();
            model.Date = string.IsNullOrEmpty(date) ? 
                    DateTime.Now : DateTime.Parse(date);
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
                new { date = model.Date.ToString("dd/MM/yyyy HH:mm"), idRegularUser = idRegularUser }
            );
        }

        [HttpGet]
        [Authorize(Policy = "RegularOnly")]
        public IActionResult EditAlimentTrack(Guid idAliment,
            Guid idRegularUser, string date)
        {
            var model = Service.GetEditAlimentTrackModel(idAliment, idRegularUser, DateTime.Parse(date));
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
                new { date = model.Date.ToString("dd/MM/yyyy HH:mm"), idRegularUser = idRegularUser }
            );
        }

        [HttpGet]
        [Authorize(Policy = "RegularOnly")]
        public IActionResult DeleteAlimentTrack(Guid idAliment,
            Guid idRegularUser, string date)
        {
            Service.DeleteAlimentTrack(idAliment, idRegularUser, DateTime.Parse(date));  
            return RedirectToAction(
                "DailyTrack",
                "NutritionTrack",
                new { date = date, idRegularUser = idRegularUser }
            );
        }

        [HttpGet]
        [Authorize(Policy = "RegularOnly")]
        public IActionResult ViewCalendar(Guid idRegularUser, int year, int month)
        {
            var model = Service.GetViewMonthCalendar(idRegularUser, year, month);
            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "RegularOnly")]
        public void ChangeActiveDay(Guid idRegularUser, string date)
        {
            Service.ChangeActiveDay(idRegularUser, DateTime.Parse(date));
        }

        [HttpGet]
        [Authorize(Policy = "RegularOnly")]
        public IActionResult NavigateMonth(Guid idRegularUser, bool prev, string monthName, int year)
        {
            var newDate = Service.NavigateMonth(prev, monthName, year);

            return RedirectToAction(
                "ViewCalendar",
                "NutritionTrack",
                new { idRegularUser = idRegularUser, year = newDate.Year, month = newDate.Month }
            );
        }
    }
}
