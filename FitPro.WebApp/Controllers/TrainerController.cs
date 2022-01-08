using FitPro.BusinessLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FitPro.WebApp.Controllers
{
    public class TrainerController : BaseController
    {
        private readonly TrainerService Service;

        public TrainerController(ControllerDependencies dependencies, TrainerService service) : base(dependencies)
        {
            Service = service;
        }

        public List<WorkoutModel> GetWorkoutsList(int currentPage, string FilterJsonString)
        {
            var filter = JsonConvert.DeserializeObject<FiltersModel>(FilterJsonString);
            var list = Service.GetWorkoutsList(CurrentUser.Id, currentPage, filter);
            return list;
        }

        public IActionResult TrainerWorkoutsList()
        {
            var model = Service.GetWorkoutsListModel(CurrentUser.Id);
            return View(model);
        }

        [HttpPost]
        public FiltersModel AddFilter(WorkoutsListModel model)
        {
            return model.FilterMore;
        } 

        [HttpGet]
        [Authorize(Policy = "TrainerOnly")]
        public IActionResult AddWorkout()
        {
            var model = new AddWorkoutModel();
            model.Categories = Service.PopulateWorkoutCategoryDropDown();
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "TrainerOnly")]
        public IActionResult AddWorkout(AddWorkoutModel model)
        {
            model.Categories = Service.PopulateWorkoutCategoryDropDown();
            model.IdTrainer = CurrentUser.Id;
            Service.AddWorkout(model);
            return RedirectToAction("TrainerWorkoutsList", "Trainer");
        }

        [HttpGet]
        public IActionResult DetailWorkout(string workoutLink, Guid programId, bool fromSaved, bool fromShare)
        {
            var model = Service.GetDetailWorkoutModel(workoutLink, programId, CurrentUser.Id, fromSaved, fromShare);
            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "TrainerOnly")]
        public IActionResult EditWorkout(string workoutLink)
        {
            var model = Service.GetEditWorkoutModel(workoutLink);
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "TrainerOnly")]
        public IActionResult EditWorkout(EditWorkoutModel model)
        {
            model.LastModifiedBy = CurrentUser.Id;
            Service.PopulateWorkoutCategoryDropDown(model);
            Service.EditWorkout(model);
            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "TrainerOnly")]
        public IActionResult DeleteWorkout(string workoutLink)
        {
            var model = Service.GetDeleteWorkoutModel(workoutLink, CurrentUser.Id);
            if (model.IdTrainer == CurrentUser.Id)
            {
                return View(model);
            }

            return RedirectToAction("DeleteWorkoutNotPermited", "Trainer", model);
        }

        [HttpPost]
        [Authorize(Policy = "TrainerOnly")]
        public IActionResult DeleteWorkout(DeleteWorkoutModel model)
        {
            Service.DeleteWorkout(model);

            return RedirectToAction("TrainerWorkoutsList", "Trainer");
        }

        [HttpGet]
        [Authorize(Policy = "TrainerOnly")]
        public IActionResult DeleteWorkoutNotPermited(DeleteWorkoutModel model)
        {
            return View(model);
        }
    }
}
