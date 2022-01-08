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
        public IActionResult Friends(Guid userId)
        {
            var model = Service.GetFriendsListModel(userId);
            return View(model);
        }

        [Authorize(Policy = "RegularOnly")]
        public List<FriendModel> GetFriends(Guid currentUserId,
            int currentPage, string searchStringFriends)
        {
            return Service.GetFriendsList(currentUserId, currentPage, searchStringFriends);
        }

        [Authorize(Policy = "RegularOnly")]
        public List<PossibleFriendModel> GetPossibleFriends(Guid currentUserId,
            int currentPage, string searchStringPossibleFriends)
        {
            return Service.GetPossibleFriendsList(currentUserId, currentPage, searchStringPossibleFriends);
        }

        [Authorize(Policy = "RegularOnly")]
        public List<FriendRequestModel> GetFriendRequestsList(Guid currentUserId, int currentPage)
        {
            return Service.GetRequestedFriendsList(currentUserId, currentPage);
        }

        [Authorize(Policy = "RegularOnly")]
        public void AddFriend(Guid currentUserId, Guid idUser)
        {
            Service.AddFriend(idUser, currentUserId);
        }

        [Authorize(Policy = "RegularOnly")]
        public void DeleteFriend(Guid currentUserId, Guid idUser)
        {
            Service.DeleteFriend(currentUserId, idUser);
        }

        [Authorize(Policy = "RegularOnly")]
        public void AcceptFriendRequest(Guid currentUserId, Guid idUser)
        {
            Service.AcceptFriendRequest(currentUserId, idUser);
        }

        [Authorize(Policy = "RegularOnly")]
        public void DeclineFriendRequest(Guid currentUserId, Guid idUser)
        {
            Service.DeclineFriendRequest(currentUserId, idUser);
        }

        [Authorize(Policy = "RegularOnly")]
        public void RemoveFriendRequest(Guid currentUserId, Guid idUser)
        {
            Service.RemoveFriendRequest(currentUserId, idUser);
        }

        [Authorize(Policy = "RegularOnly")]
        public void BlockUser(Guid currentUserId, Guid idUser)
        {
            Service.BlockUser(currentUserId, idUser);
        }

        [Authorize(Policy = "RegularOnly")]
        public void UnblockUser(Guid currentUserId, Guid idUser)
        {
            Service.UnblockUser(currentUserId, idUser);
        }

        //--------------------------------------------Programs-----------------------------------------------
        [HttpGet]
        [Authorize(Policy = "RegularOnly")]
        public IActionResult FitProProgram(Guid userId)
        {
            return View(FitProProgramService.GetUserCurrentPrograms(userId));
        }

        [HttpGet]
        [Authorize(Policy = "RegularOnly")]
        public IActionResult CreateProgram(Guid userId)
        {
            var model = new CreateProgramModel();
            FitProProgramService.PopulateWorkoutCategoryDropDown(model);
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "RegularOnly")]
        public IActionResult CreateProgram(Guid userId, CreateProgramModel  model)
        {
            FitProProgramService.PopulateWorkoutCategoryDropDown(model);
            FitProProgramService.CreateFitProProgram(userId, model);

            return RedirectToAction("FitProProgram", "User", userId);
        }

        [HttpGet]
        public IActionResult CurrentFitProProgram(Guid currentUserId, Guid programId)
        {
            var model = FitProProgramService.GetUserCurrentProgram(currentUserId, programId);
            return View(model);
        }

        public List<ProgramWorkoutModel> GetProgramWorkouts(Guid programId, int currentPage)
        {
            return FitProProgramService.GetProgramWorkouts(programId, currentPage);
        }
    }
}
