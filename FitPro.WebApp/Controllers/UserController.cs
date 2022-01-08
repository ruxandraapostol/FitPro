using FitPro.BusinessLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FitPro.WebApp.Controllers
{
    public class UserController : BaseController
    {
        private UserService Service;
        private FitProProgramService FitProProgramService;

        public UserController(ControllerDependencies dependencies, UserService service, FitProProgramService fitProProgramService) : base(dependencies)
        {
            Service = service;
            FitProProgramService = fitProProgramService;
        }



        //--------------------------------------------Save-----------------------------------------------
        [HttpGet]
        [Authorize(Policy = "RegularOnly")]
        public IActionResult SavedItems()
        {
            var model = Service.GetSavedItems(CurrentUser.Id, 1);
            return View(model);
        }

        [Authorize(Policy = "RegularOnly")]
        public List<SavedItemModel> GetSavedItemsList (int currentPage)
        {
            return Service.GetSavedItems(CurrentUser.Id, currentPage);
        }

        [Authorize(Policy = "RegularOnly")]
        public SavedItemModel GetOneSavedItemItem(int currentPage)
        {
            return Service.GetOneSavedItem(CurrentUser.Id, currentPage);
        }


        [Authorize(Policy = "RegularOnly")]
        public void SaveItem(Guid itemId)
        {
            Service.SaveItem(CurrentUser.Id, itemId);
        }

        [Authorize(Policy = "RegularOnly")]
        public void UnsaveItem(Guid itemId)
        {
            Service.UnsaveItem(CurrentUser.Id, itemId);
        }

        //--------------------------------------------Share-----------------------------------------------
        [HttpGet]
        [Authorize(Policy = "RegularOnly")]
        public IActionResult RecommandItem(Guid itemId, string fromPage)
        {
            var model = Service.GetRecommandItemModel(CurrentUser.Id, itemId, fromPage);
            return View(model);
        }

        [Authorize(Policy = "RegularOnly")]
        public void ShareContent(string jsonModel, string jsonFriendsUserNames)
        {
            var list = JsonConvert.DeserializeObject<List<string>>(jsonFriendsUserNames);
            var model = JsonConvert.DeserializeObject<RecommandItemModel>(jsonModel);
            model.CurrentUserId = CurrentUser.Id;
            Service.RecommandSomething(model, list);
        }

        [HttpGet]
        [Authorize(Policy = "RegularOnly")]
        public IActionResult MyRecommandation()
        {
            var model = Service.GetFiendRecommands(CurrentUser.Id, 1);
            return View(model);
        }


        [Authorize(Policy = "RegularOnly")]
        public List<FriendRecommand> GetFriendsRecommandations(int currentPage)
        {
            return Service.GetFiendRecommands(CurrentUser.Id, currentPage);
        }

        [Authorize(Policy = "RegularOnly")]
        public List<FriendRecommand> GetMyRecommandations(int currentPage)
        {
            return Service.GetMyRecommands(CurrentUser.Id, currentPage);
        }


        //--------------------------------------------Friends-----------------------------------------------

        [HttpGet]
        [Authorize(Policy = "RegularOnly")]
        public IActionResult Friends()
        {
            var model = Service.GetFriendsListModel(CurrentUser.Id);
            return View(model);
        }

        [Authorize(Policy = "RegularOnly")]
        public List<FriendModel> GetFriends(int currentPage, string searchStringFriends)
        {
            return Service.GetFriendsList(CurrentUser.Id, currentPage, searchStringFriends);
        }

        [Authorize(Policy = "RegularOnly")]
        public List<PossibleFriendModel> GetPossibleFriends(
            int currentPage, string searchStringPossibleFriends)
        {
            return Service.GetPossibleFriendsList(CurrentUser.Id, currentPage, searchStringPossibleFriends);
        }

        [Authorize(Policy = "RegularOnly")]
        public List<FriendRequestModel> GetFriendRequestsList(int currentPage)
        {
            return Service.GetRequestedFriendsList(CurrentUser.Id, currentPage);
        }

        [Authorize(Policy = "RegularOnly")]
        public void AddFriend(Guid idUser)
        {
            Service.AddFriend(idUser, CurrentUser.Id);
        }

        [Authorize(Policy = "RegularOnly")]
        public void DeleteFriend(Guid idUser)
        {
            Service.DeleteFriend(CurrentUser.Id, idUser);
        }

        [Authorize(Policy = "RegularOnly")]
        public void AcceptFriendRequest(Guid idUser)
        {
            Service.AcceptFriendRequest(CurrentUser.Id, idUser);
        }

        [Authorize(Policy = "RegularOnly")]
        public void DeclineFriendRequest(Guid idUser)
        {
            Service.DeclineFriendRequest(CurrentUser.Id, idUser);
        }

        [Authorize(Policy = "RegularOnly")]
        public void RemoveFriendRequest(Guid idUser)
        {
            Service.RemoveFriendRequest(CurrentUser.Id, idUser);
        }

        [Authorize(Policy = "RegularOnly")]
        public void BlockUser(Guid idUser)
        {
            Service.BlockUser(CurrentUser.Id, idUser);
        }

        [Authorize(Policy = "RegularOnly")]
        public void UnblockUser(Guid idUser)
        {
            Service.UnblockUser(CurrentUser.Id, idUser);
        }

        //--------------------------------------------Programs-----------------------------------------------
        [HttpGet]
        [Authorize(Policy = "RegularOnly")]
        public IActionResult FitProProgram()
        {
            return View(FitProProgramService.GetUserCurrentPrograms(CurrentUser.Id));
        }

        [HttpGet]
        [Authorize(Policy = "RegularOnly")]
        public IActionResult CreateProgram()
        {
            var model = new CreateProgramModel();
            FitProProgramService.PopulateWorkoutCategoryDropDown(model);
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "RegularOnly")]
        public IActionResult CreateProgram(CreateProgramModel  model)
        {
            FitProProgramService.PopulateWorkoutCategoryDropDown(model);
            FitProProgramService.CreateFitProProgram(CurrentUser.Id, model);

            return RedirectToAction("FitProProgram", "User");
        }

        [HttpGet]
        public IActionResult CurrentFitProProgram(Guid programId)
        {
            var model = FitProProgramService.GetUserCurrentProgram(CurrentUser.Id, programId);
            return View(model);
        }

        public List<ProgramWorkoutModel> GetProgramWorkouts(Guid programId, int currentPage)
        {
            return FitProProgramService.GetProgramWorkouts(programId, currentPage);
        }
    }
}
