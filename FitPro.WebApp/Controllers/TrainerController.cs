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

        public List<WorkoutModel> GetWorkoutsList(Guid currentUserId, int currentPage, string FilterJsonString)
        {
            var filter = JsonConvert.DeserializeObject<FiltersModel>(FilterJsonString);
            var list = Service.GetWorkoutsList(currentUserId, currentPage, filter);
            return list;
        }

        public IActionResult TrainerWorkoutsList(Guid currentId)
        {
            var model = Service.GetWorkoutsListModel(currentId);
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
        public IActionResult AddWorkout(AddWorkoutModel model, Guid idTrainer)
        {
            model.Categories = Service.PopulateWorkoutCategoryDropDown();
            model.IdTrainer = idTrainer;
            Service.AddWorkout(model);
            return RedirectToAction("TrainerWorkoutsList", "Trainer");
        }

        [HttpGet]
        public IActionResult DetailWorkout(string workoutLink, Guid programId, Guid userId, bool fromSaved, bool fromShare)
        {
            var model = Service.GetDetailWorkoutModel(workoutLink, programId, userId, fromSaved, fromShare);
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
        public IActionResult EditWorkout(EditWorkoutModel model, Guid idCurrentTrainer)
        {
            model.LastModifiedBy = idCurrentTrainer;
            Service.PopulateWorkoutCategoryDropDown(model);
            Service.EditWorkout(model);
            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "TrainerOnly")]
        public IActionResult DeleteWorkout(string workoutLink, Guid currentId)
        {
            var model = Service.GetDeleteWorkoutModel(workoutLink, currentId);
            if (model.IdTrainer == currentId)
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
