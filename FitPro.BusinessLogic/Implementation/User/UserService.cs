using FitPro.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
namespace FitPro.BusinessLogic
{
    public class UserService : BaseService
    {
        public UserService(ServiceDependencies serviceDependencies) : base(serviceDependencies)
        {

        }

        public List<SavedItemModel> GetSavedItems(Guid currentUserId, int currentPage)
        {
            var savedItems = UnitOfWork.Saved.Get()
                .Include(x => x.IdRecipeNavigation)
                .Include(x => x.IdWorkoutNavigation)
                .OrderByDescending(x => x.Date)
                .Where(x => x.IdRegularUser == currentUserId);

            savedItems = (currentPage > 1) ? savedItems.Skip((currentPage - 1) * 12) : savedItems;

            var savedItemsList = new List<SavedItemModel>();

            savedItems.Take(12).ToList().ForEach(x => { savedItemsList.Add(CompleteSavedItem(x)); });

            return savedItemsList;
        }


        public SavedItemModel GetOneSavedItem(Guid currentUserId, int currentPage)
        {
            var savedItems = UnitOfWork.Saved.Get()
                .Include(x => x.IdRecipeNavigation)
                .Include(x => x.IdWorkoutNavigation)
                .OrderByDescending(x => x.Date)
                .Where(x => x.IdRegularUser == currentUserId);

            savedItems = (currentPage > 1) ? savedItems.Skip((currentPage - 1) * 12 - 1) : savedItems;

            return CompleteSavedItem(savedItems.FirstOrDefault());
        }  


        public void SaveItem(Guid currentUserId, Guid itemId)
        {
            ExecuteInTransaction(uow =>
            {
                var isWorkout = uow.Workouts.Get()
                    .SingleOrDefault(x => x.IdWorkout == itemId);

                var newSaved = new Saved()
                {
                    IdSaved = Guid.NewGuid(),
                    IdRegularUser = currentUserId,
                    Date = DateTime.Now,
                };

                if (isWorkout != null)
                {
                    newSaved.IdWorkout = itemId;
                    uow.Saved.Insert(newSaved);
                } else
                {
                    var isRecipe = uow.Recipes.Get()
                    .SingleOrDefault(x => x.IdRecipe == itemId);

                    if (isRecipe != null)
                    {
                        newSaved.IdRecipe = itemId;
                        uow.Saved.Insert(newSaved);
                    }
                }
                uow.SaveChanges();
            });
        }

        public void UnsaveItem(Guid currentUserId, Guid itemId)
        {
            ExecuteInTransaction(uow =>
            {
                var savedItem = uow.Saved.Get()
                    .SingleOrDefault(x => x.IdRegularUser == currentUserId &&
                    (x.IdWorkout == itemId || x.IdRecipe == itemId));

                if (savedItem != null)
                {
                    uow.Saved.Delete(savedItem);
                    uow.SaveChanges();
                }
            });
        }

        public RecommandItemModel GetRecommandItemModel(Guid currentUser, Guid idItem, string fromPage)
        {
            var model = new RecommandItemModel() { 
                IdItem = idItem, 
                CurrentUserId = currentUser,
                FromPage = fromPage
            };

            var workout = UnitOfWork.Workouts.Get()
                    .SingleOrDefault(x => x.IdWorkout == model.IdItem);
            model.IsWorkout = workout != null;
            if (model.IsWorkout)
            {
                model.Name = workout.Name;
                model.Link = workout.LinkUrl;
            }
            else
            {
                model.Name = UnitOfWork.Recipes.Get()
                    .SingleOrDefault(x => x.IdRecipe == model.IdItem)?.Name;
            }

            model.FriendsList = GetFriendsList(currentUser, 1, null);
            return model;
        }

        public void RecommandSomething(RecommandItemModel model, List<string> SelectedFriends)
        {
            ExecuteInTransaction(uow =>
            {
                var isWorkout = uow.Workouts.Get()
                    .SingleOrDefault(x => x.IdWorkout == model.IdItem);

                foreach (var friend in SelectedFriends)
                {
                    var newRecommandation = new Recommandation()
                    {
                        IdRecommandation = Guid.NewGuid(),
                        IdFromUser = model.CurrentUserId,
                        IdToUser = GetUserIdByUsername(friend),
                        SendDate = DateTime.Now,
                        Comment = model.Comment
                    };

                    if (isWorkout != null)
                    {
                        newRecommandation.IdWorkout = model.IdItem;                        
                    }
                    else
                    {
                        var isRecipe = uow.Recipes.Get()
                        .SingleOrDefault(x => x.IdRecipe == model.IdItem);

                        if (isRecipe != null)
                        {
                            newRecommandation.IdRecipe = model.IdItem;
                        }
                    }
                    uow.Recommandations.Insert(newRecommandation);
                }
                uow.SaveChanges();
            });
        }

        private List<FriendRecommand> CreateFriendRecommandList(IQueryable<Recommandation> recommandations,bool fromCurrentUser, int currentPage)
        {
            if (currentPage > 1)
            {
                recommandations = recommandations.Skip((currentPage - 1) * 15);
            }

            recommandations = recommandations.Take(15);

            var list = recommandations.ToList();
            var FriendRecommandList = new List<FriendRecommand>();
            list.ForEach(x =>
            {
                var newFriendRecommand = new FriendRecommand()
                {
                    Comment = x.Comment,
                    Date = x.SendDate.ToShortDateString(),
                };

                if(fromCurrentUser)
                {
                    newFriendRecommand.FriendUserName = x.IdFromUserNavigation.IdRegularUserNavigation.UserName;
                } else
                {
                    newFriendRecommand.FriendUserName = x.IdToUserNavigation.IdRegularUserNavigation.UserName;
                }

                if (x.IdWorkoutNavigation != null)
                {
                    newFriendRecommand.IsWorkout = true;
                    newFriendRecommand.ItemId = x.IdWorkout ?? Guid.Empty;
                    newFriendRecommand.Name = x.IdWorkoutNavigation.Name;
                    newFriendRecommand.Link = x.IdWorkoutNavigation.LinkUrl;
                }
                else
                {
                    newFriendRecommand.IsWorkout = false;
                    newFriendRecommand.ItemId = x.IdRecipe ?? Guid.Empty;
                    newFriendRecommand.Name = x.IdRecipeNavigation.Name;
                }

                FriendRecommandList.Add(newFriendRecommand);
            });

            return FriendRecommandList;
        }

        public List<FriendRecommand> GetFiendRecommands(Guid currentUserId, int currentPage)
        {
            var recommandations = UnitOfWork.Recommandations.Get()
                .Include(x => x.IdWorkoutNavigation)
                .Include(x => x.IdRecipeNavigation)
                .Include(x => x.IdFromUserNavigation)
                .ThenInclude(x => x.IdRegularUserNavigation)
                .OrderBy(x => x.SendDate)
                .Where(x => x.IdToUser == currentUserId);

            return CreateFriendRecommandList(recommandations, true,  currentPage);
        }

        public List<FriendRecommand> GetMyRecommands(Guid currentUserId, int currentPage)
        {
            var recommandations = UnitOfWork.Recommandations.Get()
                .Include(x => x.IdWorkoutNavigation)
                .Include(x => x.IdRecipeNavigation)
                .Include(x => x.IdToUserNavigation)
                .ThenInclude(x => x.IdRegularUserNavigation)
                .OrderBy(x => x.SendDate)
                .Where(x => x.IdFromUser == currentUserId);

            return CreateFriendRecommandList(recommandations, false, currentPage);
        }

        public FriendsListModel GetFriendsListModel(Guid Id)
        {
            var model = new FriendsListModel();
            model.IdUser = Id;

            var friendsList1 = GetRowFriends1(model.IdUser, "", 1);
            var friendsList2 = GetRowFriends2(model.IdUser, "", 1);
            model.FriendsList = friendsList1.Concat(friendsList2).ToList();

            return model;
        }
        
        public List<FriendModel> GetFriendsList(Guid currentUserId,
            int currentPage, string searchStringFriends)
        {
            var friendsList1 = GetRowFriends1(currentUserId, searchStringFriends, currentPage);
            var friendsList2 = GetRowFriends2(currentUserId, searchStringFriends, currentPage);

            return friendsList1.Concat(friendsList2).ToList();
        }

        public void AddFriend(Guid idToUser, Guid idFromUser)
        {
            ExecuteInTransaction(uow =>
            {
                var newRequest = uow.Requests.Get()
                .SingleOrDefault(x => (x.IdFromUser == idFromUser && x.IdToUser == idToUser));

                if (newRequest == null)
                {
                    newRequest = new Request()
                    {
                        IdFromUser = idFromUser,
                        IdToUser = idToUser,
                        Status = (int)Status.Waiting,
                        Date = DateTime.Now,
                    };

                    var oldRequest = uow.Requests.Get()
                        .SingleOrDefault(x => (x.IdFromUser == idToUser && x.IdToUser == idFromUser));

                    if(oldRequest != null)
                    {
                        uow.Requests.Delete(oldRequest);
                    }

                    uow.Requests.Insert(newRequest);
                    uow.SaveChanges();
                }
                else if(newRequest.Status == (int)Status.Nothing)
                {
                    newRequest.Status = (int)Status.Waiting;
                    uow.Requests.Update(newRequest);
                    uow.SaveChanges();
                }
            });
        }
        
        public void AcceptFriendRequest(Guid idToUser, Guid idFromUser)
        {
            ExecuteInTransaction(uow =>
            {
                var request = uow.Requests.Get()
                    .SingleOrDefault(x => x.IdFromUser == idFromUser
                    && x.IdToUser == idToUser);

                request.Status = (int)Status.Friend;
                uow.Requests.Update(request);

                var newFriendship = new FriendsList()
                {
                    IdUser2 = (idToUser.CompareTo(idFromUser) < 0) ? idToUser : idFromUser,
                    IdUser1 = (idToUser.CompareTo(idFromUser) > 0) ? idToUser : idFromUser
                };

                uow.FriendsLists.Insert(newFriendship);
                uow.SaveChanges();
            });
        }

        public void DeclineFriendRequest(Guid idToUser, Guid idFromUser)
        {
            ExecuteInTransaction(uow =>
            {
                var request = uow.Requests.Get()
                    .SingleOrDefault(x => x.IdFromUser == idFromUser
                    && x.IdToUser == idToUser);

                request.Status = (int)Status.Nothing;
                uow.Requests.Update(request);
                uow.SaveChanges();
            });
        }

        public void BlockUser(Guid idUser, Guid idFriend)
        {
            ExecuteInTransaction(uow =>
            {
                var request = uow.Requests.Get()
                    .SingleOrDefault(x => x.IdFromUser == idUser && x.IdToUser == idFriend);

                if (request != null)
                {
                    request.Status = (int)Status.Blocked;
                    request.Date = DateTime.Now;
                    uow.Requests.Update(request);
                } else
                {
                    request = uow.Requests.Get()
                    .SingleOrDefault(x => x.IdFromUser == idFriend && x.IdToUser == idUser);
                    uow.Requests.Delete(request);
                    uow.Requests.Insert(new Request()
                    {
                        IdFromUser = idUser,
                        IdToUser = idFriend,
                        Status = (int)Status.Blocked,
                        Date = DateTime.Now
                    });
                }

                var friendship = uow.FriendsLists.Get()
                    .SingleOrDefault(x => (x.IdUser1 == idFriend && x.IdUser2 == idUser) 
                    || (x.IdUser1 == idUser && x.IdUser2 == idFriend));

                if(friendship != null)
                {
                    uow.FriendsLists.Delete(friendship);
                }   
                uow.SaveChanges();
            });
        }

        public void UnblockUser(Guid idUser, Guid idFriend)
        {
            ExecuteInTransaction(uow =>
            {
                var request = uow.Requests.Get()
                    .SingleOrDefault(x => x.Status == (int)Status.Blocked && ((x.IdFromUser == idFriend 
                    && x.IdToUser == idUser) || (x.IdFromUser == idUser && x.IdToUser == idFriend)));

                request.Status = (int)Status.Nothing;
                uow.Requests.Update(request);
                uow.SaveChanges();
            });
        }

        public void DeleteFriend(Guid idUser, Guid idFriend)
        {
            ExecuteInTransaction(uow =>
            {
                var request = uow.Requests.Get()
                    .SingleOrDefault(x => (x.IdFromUser == idFriend && x.IdToUser == idUser) 
                    || (x.IdFromUser == idUser && x.IdToUser == idFriend));

                request.Status = (int)Status.Nothing;
                uow.Requests.Update(request);

                var friendship = uow.FriendsLists.Get()
                    .SingleOrDefault(x => (x.IdUser1 == idFriend && x.IdUser2 == idUser)
                    || (x.IdUser2 == idFriend && x.IdUser1 == idUser));

                if(friendship != null)
                {
                    uow.FriendsLists.Delete(friendship);
                }

                uow.SaveChanges();
            });
        }

        public void RemoveFriendRequest(Guid idUser, Guid idFriend)
        {
            ExecuteInTransaction(uow =>
            {
                var request = uow.Requests.Get()
                    .SingleOrDefault(x => x.IdFromUser == idUser && x.IdToUser == idFriend 
                        && x.Status == (int)Status.Waiting);

                request.Status = (int)Status.Nothing;
                uow.Requests.Update(request);
                uow.SaveChanges();
            });
        }

        public List<PossibleFriendModel> GetPossibleFriendsList(Guid currentUserId,
            int currentPage, string searchStringPossibleFriends)
        {
            if (searchStringPossibleFriends == null)
            {
                return null;
            }

            var lowSearhString = searchStringPossibleFriends.ToLower();

            var requestList = UnitOfWork.Requests.Get()
                .Where(x => x.IdToUser == currentUserId && x.Status == (int)Status.Blocked)
                .Select(x => x.IdFromUser)
                .ToList();

            var list = UnitOfWork.Users.Get()
                .Include(x => x.IdRoleNavigation)
                .Where(x => x.IdUser != currentUserId
                    && x.IdRoleNavigation.Name == "Regular" && x.Alive == true
                    && !requestList.Contains(x.IdUser));

            if(lowSearhString != null)
            {
                list = list.Where(x => x.UserName.ToLower().Contains(lowSearhString));
            }

            List<PossibleFriendModel> onlyFivePossibleFriends;

            if(currentPage > 1)
            {
                onlyFivePossibleFriends = list.Skip((currentPage - 1) * 5).Take(5)
                .Select(x => Mapper.Map<User,PossibleFriendModel>(x))
                .ToList();
            } else
            {
                onlyFivePossibleFriends = list.Take(5)
                .Select(x => Mapper.Map<User, PossibleFriendModel>(x))
                .ToList();
            }

            onlyFivePossibleFriends.ForEach(x => x.Status = GetPossibleFriendStatus(x, currentUserId));

            return onlyFivePossibleFriends;
        }

        public List<FriendRequestModel> GetRequestedFriendsList(Guid currentUserId,
            int currentPage)
        {
            var list = UnitOfWork.Requests.Get()
                .Include(x => x.IdFromUserNavigation)
                .ThenInclude(x => x.IdRegularUserNavigation)
                .Where(x => x.IdToUser == currentUserId && x.Status == ((int)Status.Waiting))
                .OrderByDescending(x => x.Date)
                .Select(x => new FriendRequestModel()
                {
                    IdUser = x.IdFromUser,
                    UserName = x.IdFromUserNavigation.IdRegularUserNavigation.UserName
                });

            if (list.Count() == 0)
            {
                return null;
            }

            return (currentPage > 1) ? list.Skip((currentPage - 1) * 2).Take(2).ToList() : list.Take(2).ToList();
        }

        private string GetPossibleFriendStatus(PossibleFriendModel elem, Guid IdCurrentUser)
        {
            var possibleRequestFromUser = UnitOfWork.Requests.Get()
                        .SingleOrDefault(r => r.IdToUser == elem.IdUser && r.IdFromUser == IdCurrentUser);

            if (possibleRequestFromUser != null)
            {
                switch (possibleRequestFromUser.Status)
                {
                    case (int)Status.Waiting:
                        return "WaitingForResponse";
                    case (int)Status.Friend:
                        return "AlreadyFriends";
                    case (int)Status.Blocked:
                        return "BlockedByCurrentUser";
                    default:
                        return "Nothing";
                }
            }
            else
            {
                var possibleRequestToUser = UnitOfWork.Requests.Get()
                .SingleOrDefault(r => r.IdFromUser == elem.IdUser && r.IdToUser == IdCurrentUser);

                if (possibleRequestToUser != null)
                {
                    switch (possibleRequestToUser.Status)
                    {
                        case (int)Status.Waiting:
                            return "AcceptFriendship";
                        case (int)Status.Friend:
                            return "AlreadyFriends";
                        default:
                            return "Nothing";
                    }
                }
            }

            return "Nothing";
        }

        private List<FriendModel> GetRowFriends1 (Guid currentId, string searchStringFriend, int currentPage) 
        {
            var lowSearhString = (searchStringFriend == null) ? null : searchStringFriend.ToLower();

            var listUser1 = UnitOfWork.FriendsLists.Get()
                .Include(x => x.IdUser2Navigation)
                .ThenInclude(x => x.IdRegularUserNavigation)
                .Where(x => x.IdUser1 == currentId);

            if(lowSearhString != null)
            {
                listUser1 = listUser1.Where(x => x.IdUser2Navigation.IdRegularUserNavigation
                    .UserName.ToLower().Contains(lowSearhString));
            }

            if (currentPage > 1)
            {
                listUser1 = listUser1.Skip((currentPage - 1) * 5);
            }
            
            return listUser1.Take(5)
                .Select(x => Mapper.Map<RegularUser, FriendModel>(x.IdUser2Navigation))
                .ToList();
        }

        private List<FriendModel> GetRowFriends2(Guid currentId, string searchStringFriend, int currentPage)
        {
            var lowSearhString = (searchStringFriend == null) ? null : searchStringFriend.ToLower();

            var listUser2 = UnitOfWork.FriendsLists.Get()
                .Include(x => x.IdUser1Navigation)
                .ThenInclude(x => x.IdRegularUserNavigation)
                .Where(x => x.IdUser2 == currentId);

            if (lowSearhString != null)
            {
                listUser2 = listUser2.Where(x => x.IdUser1Navigation.IdRegularUserNavigation
                    .UserName.ToLower().Contains(lowSearhString));
            }

            if (currentPage > 1)
            {
                listUser2 = listUser2.Skip((currentPage - 1) * 5);
            }

            return listUser2.Take(5)
                .Select(x => Mapper.Map<RegularUser, FriendModel>(x.IdUser1Navigation))
                .ToList();
        }
    
        private SavedItemModel CompleteSavedItem(Saved item)
        {
            if(item == null)
            {
                return null;
            }

            if (item.IdWorkout != Guid.Empty && item.IdWorkout != null)
            {
                return new SavedItemModel()
                {
                    IsWorkout = true,
                    IdSavedItem = item.IdWorkout,
                    Name = item.IdWorkoutNavigation.Name,
                    Link = item.IdWorkoutNavigation.LinkUrl
                };
            }
            
            return new SavedItemModel()
                {
                    IsWorkout = false,
                    IdSavedItem = item.IdRecipe,
                    Name = item.IdRecipeNavigation.Name
                };
        }
    
        
    }
}
