using System;
using System.Collections.Generic;
using FitPro.BusinessLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitPro.WebApp.Controllers
{
    public class NutritionTrackController : BaseController
    {
        private readonly NutritionTrackService Service;
        public NutritionTrackController(ControllerDependencies dependencies,
            NutritionTrackService service) : base(dependencies)
        {
            this.Service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Policy = "RegularOnly")]
        public IActionResult DailyTrack(string date = "")
        {
            var actualDate = string.IsNullOrEmpty(date) ?
                    DateTime.Now : DateTime.Parse(date);
            
            var model = Service.GetDailyList(actualDate, CurrentUser.Id);
            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "RegularOnly")]
        public IActionResult NavigateDay(bool prev, string date = "")
        {
            var actualDate = string.IsNullOrEmpty(date) ?
                    DateTime.Now : DateTime.Parse(date);

            var prevDay = prev? actualDate.AddDays(-1) : actualDate.AddDays(1);

            return RedirectToAction(
                "DailyTrack",
                "NutritionTrack",
                new { date = prevDay.ToString("dd/MM/yyyy HH:mm")}
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
        public IActionResult AddAlimentTrack(SaveAlimentTrackModel model)
        {
            model.IdRegularUser = CurrentUser.Id;
            model.FoodList = Service.PopulateFoodList();
            Service.AddAlimentTrack(model);
            return RedirectToAction(
                "DailyTrack", 
                "NutritionTrack", 
                new { date = model.Date.ToString("dd/MM/yyyy HH:mm")}
            );
        }

        [HttpGet]
        [Authorize(Policy = "RegularOnly")]
        public IActionResult EditAlimentTrack(Guid idAliment, string date)
        {
            var model = Service.GetEditAlimentTrackModel(idAliment, CurrentUser.Id, DateTime.Parse(date));
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "RegularOnly")]
        public IActionResult EditAlimentTrack(SaveAlimentTrackModel model)
        {
            model.IdRegularUser = CurrentUser.Id;
            Service.EditAlimentTrack(model);
            return RedirectToAction(
                "DailyTrack",
                "NutritionTrack",
                new { date = model.Date.ToString("dd/MM/yyyy HH:mm")}
            );
        }

        [HttpGet]
        [Authorize(Policy = "RegularOnly")]
        public IActionResult DeleteAlimentTrack(Guid idAliment, string date)
        {
            Service.DeleteAlimentTrack(idAliment, CurrentUser.Id, DateTime.Parse(date));  
            return RedirectToAction(
                "DailyTrack",
                "NutritionTrack",
                new { date = date}
            );
        }

        [HttpGet]
        [Authorize(Policy = "RegularOnly")]
        public IActionResult ViewCalendar(int year, int month)
        {
            var model = Service.GetViewMonthCalendar(CurrentUser.Id, year, month);
            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "RegularOnly")]
        public void ChangeActiveDay(string date)
        {
            Service.ChangeActiveDay(CurrentUser.Id, DateTime.Parse(date));
        }

        [HttpGet]
        [Authorize(Policy = "RegularOnly")]
        public IActionResult NavigateMonth(bool prev, string monthName, int year)
        {
            var newDate = Service.NavigateMonth(prev, monthName, year);

            return RedirectToAction(
                "ViewCalendar",
                "NutritionTrack",
                new { year = newDate.Year, month = newDate.Month }
            );
        }

        [HttpGet]
        [Authorize(Policy = "RegularOnly")]
        public IActionResult NavigateMonthStatistics(bool prev, string monthName, int year)
        {
            var newDate = Service.NavigateMonth(prev, monthName, year);

            return RedirectToAction(
                "ViewStatistics",
                "NutritionTrack",
                new { year = newDate.Year, month = newDate.Month }
            );
        }

        [HttpGet]
        [Authorize(Policy = "RegularOnly")]
        public IActionResult ViewStatistics(int year, int month)
        {
            var model = Service.GetViewStatistics(year, month);
            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "RegularOnly")]
        public GraphModel MonthStatistic(int year, string month, int macronutrient)
        {
            return Service.GetMonthlyStatistic(CurrentUser.Id, month, year, macronutrient);
        }

        [HttpGet]
        [Authorize(Policy = "RegularOnly")]
        public GraphModel YearStatistic(int year, int macronutrient)
        {
            return Service.GetYearlyStatistic(CurrentUser.Id, year, macronutrient);
        }
    }
}
