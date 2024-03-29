﻿using Microsoft.AspNetCore.Mvc;
using FitPro.BusinessLogic;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FitPro.WebApp.Controllers
{
    public class NutritionistController : BaseController
    {
        private NutritionistService Service;

        public NutritionistController(ControllerDependencies dependencies, NutritionistService service) : base(dependencies)
        {
            Service = service;
        }

        public IActionResult AlimentsList(int currentPage, string searchString)
        {
            var model = Service.GetAlimentsListModel(currentPage, searchString);
            return View(model);
        }

        public List<DetailAlimentModel> GetAlimentsList(int currentPage, string searchString)
        {
            return Service.GetAlimentsList(currentPage, searchString);
        }

        [HttpGet]
        [Authorize(Policy = "NutritionistOnly")]
        public IActionResult AddAliment()
        {
            var model = new AddAlimentModel();
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "NutritionistOnly")]
        public IActionResult AddAliment(AddAlimentModel model)
        {
            model.IdNutritionist = CurrentUser.Id;
            Service.AddAliment(model);
            return RedirectToAction("AlimentsList", "Nutritionist", new { currnetPage = 1 });
        }

        [HttpGet]
        [Authorize(Policy = "NutritionistOnly")]
        public IActionResult EditAliment(string alimentName)
        {
            var model = Service.GetAlimentByName(alimentName);
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "NutritionistOnly")]
        public IActionResult EditAliment(EditAlimentModel model)
        {
            model.Nutritionist = CurrentUser.Id;
            Service.EditAliment(model);
            return RedirectToAction("AlimentsList", "Nutritionist", new { currnetPage = 1 });
        }

        [HttpGet]
        [Authorize(Policy = "NutritionistOnly")]
        public IActionResult DeleteAliment(string alimentName)
        {
            var model = Service.GetDeleteAlimentModelByName(alimentName);

            model.IdNutritionist = (model.IdNutritionist == Guid.Empty) ? CurrentUser.Id : model.IdNutritionist;

            return (model.IdNutritionist == CurrentUser.Id) ? View(model) : RedirectToAction("DeleteAlimentNotPermited", "Nutritionist", model);
        }

        [HttpPost]
        [Authorize(Policy = "NutritionistOnly")]
        public IActionResult DeleteAliment(DeleteAlimentModel model)
        {
            Service.DeleteAliment(model);
            return RedirectToAction("AlimentsList", "Nutritionist", new { currnetPage = 1 });
        }

        [HttpGet]
        [Authorize(Policy = "NutritionistOnly")]
        public IActionResult DeleteAlimentNotPermited(DeleteAlimentModel model)
        {
            return View(model);
        }


        /* ----------------------------------------------------------------------------------------------------------------- */

        [HttpGet]
        public List<RecipeModel> GetRecipesList(int currentPage, string FilterJsonString)
        {
            var filter = JsonConvert.DeserializeObject<FilterRecipeModel>(FilterJsonString);
            var list = Service.GetRecipesList(CurrentUser.Id, currentPage, filter);
            return list;
        }

        [HttpGet]
        public IActionResult NutritionistRecipesList()
        {
            var model = Service.GetRecipeModelList(CurrentUser.Id);
            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "NutritionistOnly")]
        public IActionResult AddRecipe()
        {
            var model = new AddRecipeModel();
            model.Categories = Service.PopulateCategoryDropDown();
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "NutritionistOnly")]
        public IActionResult AddRecipe(AddRecipeModel model)
        {
            model.IdNutritionist = CurrentUser.Id;
            Service.AddRecipe(model);
            return RedirectToAction("NutritionistRecipesList","Nutritionist");
        }

        [HttpGet]
        [Authorize(Policy = "NutritionistOnly")]
        public IActionResult EditRecipe(Guid idRecipe)
        {
            var model = Service.GetEditRecipeModelById(idRecipe);
            model.Categories = Service.PopulateCategoryDropDown();
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "NutritionistOnly")]
        public IActionResult EditRecipe(EditRecipeModel model)
        {
            model.IdNutritionist = CurrentUser.Id;
            Service.EditRecipe(model);
            return RedirectToAction("NutritionistRecipesList", "Nutritionist");
        }

        [HttpGet]
        [Authorize(Policy = "NutritionistOnly")]
        public IActionResult DeleteRecipeNotPermited(DeleteRecipeModel model)
        {
            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "NutritionistOnly")]
        public IActionResult DeleteRecipe(Guid idRecipe)
        {
            var model = Service.GetDeleteRecipeModelById(idRecipe);

            if(model.IdNutritionist != CurrentUser.Id)
            {
                return RedirectToAction("DeleteRecipeNotPermited", "Nutritionist", model);
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "NutritionistOnly")]
        public IActionResult DeleteRecipe(DeleteRecipeModel model)
        {
            Service.DeleteRecipe(model);
            return RedirectToAction("NutritionistRecipesList", "Nutritionist");
        }

        [HttpGet]
        public IActionResult DetailRecipe(Guid idRecipe, bool fromSavedItems, bool fromShare)
        {
            var model = Service.GetDetailModelById(idRecipe, fromSavedItems, fromShare);
            return View(model);
        }



        [HttpGet]
        public ActionResult RecipePic(Guid recipeId)
        {
            var image = Service.GetRecipePicOrDefault(recipeId);
            return File(image, "image/jpeg");
        }
    }
}
