using FitPro.Common;
using FitPro.Common.DTOs;
using FitPro.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WebMatrix.WebData;
using BCryptNet = BCrypt.Net.BCrypt;

namespace FitPro.BusinessLogic
{
    public class RegularUserAccountService : BaseService
    {
        private readonly LoginValidation LoginValidator;
        private readonly ChangePasswordValidation ChangePasswordValidator;
        private readonly ForgotPasswordValidation ForgotPasswordValidator;
        private readonly RegularUserRegisterValidation RegisterRegularUserValidator;
        private readonly RegularUserProfileEditingValidation EditRegularUserValidator;
        private readonly SpecialUserProfileEditingValidation EditSpecialUserValidator;

        public RegularUserAccountService(ServiceDependencies dependencies) : base(dependencies)
        {
            this.LoginValidator = new LoginValidation(this);
            this.ChangePasswordValidator = new ChangePasswordValidation(this);
            this.ForgotPasswordValidator = new ForgotPasswordValidation(this);
            this.RegisterRegularUserValidator = new RegularUserRegisterValidation(this);
            this.EditRegularUserValidator = new RegularUserProfileEditingValidation(this);
            this.EditSpecialUserValidator = new SpecialUserProfileEditingValidation(this);
        }


        public string GetRoleByUserName(string userName)
        {
            return UnitOfWork.Users.Get()
                .Include(x => x.IdRoleNavigation)
                .SingleOrDefault(x => x.UserName == userName)?
                .IdRoleNavigation.Name;
        }
       

        public DetailsRegular GetDetailsRegular(string userName)
        {
            var user = UnitOfWork.Users.Get()
                .SingleOrDefault(x => x.UserName == userName);
            
            if(user == null) 
            { 
                return null;
            }

            var details = new DetailsRegular()
            {
                UserName = userName,
                Name = user.FirstName + " " + user.LastName,
                Email = user.Email,
                FriendsNumber = UnitOfWork.FriendsLists.Get()
                    .Include(x => x.IdUser1Navigation)
                    .ThenInclude(x => x.IdRegularUserNavigation)
                    .Include(x => x.IdUser2Navigation)
                    .ThenInclude(x => x.IdRegularUserNavigation)
                    .Where(x => x.IdUser2Navigation.IdRegularUserNavigation.UserName == userName
                     || x.IdUser1Navigation.IdRegularUserNavigation.UserName == userName)
                    .Count(),
                LastSavedItems = GetUserSavedItems(user.IdUser),
                LastRecommand = GetUserSharedItems(user.IdUser)
            };

            return details;

        }


        public DetailsAdmin GetDetailsAdmin(string userName)
        {
            var user = UnitOfWork.Users.Get()
                .SingleOrDefault(x => x.UserName == userName);

            if (user == null)
            {
                return null;
            }

            var details = new DetailsAdmin()
            {
                UserName = userName,
                Name = user.FirstName + " " + user.LastName,
                Email = user.Email
            };

            return details;
        }


        public DetailsTrainer GetDetailsTrainer(string userName)
        {
            var user = UnitOfWork.Users.Get()
                .SingleOrDefault(x => x.UserName == userName);

            if (user == null)
            {
                return null;
            }

            var rawTrainersWorkouts = UnitOfWork.Workouts.Get()
                .Where(x => x.IdTrainer == user.IdUser)
                .Select(x => Mapper.Map<Workout, WorkoutModel>(x));

            var details = new DetailsTrainer()
            {
                UserName = userName,
                Name = user.FirstName + " " + user.LastName,
                Email = user.Email,
                ContributionsTotal = rawTrainersWorkouts.Count(),
                LastWorkouts = rawTrainersWorkouts.Take(3).ToList()
            };

            return details;
        }

        public DetailsNutritionist GetDetailsNutritionist(string userName)
        {
            var user = UnitOfWork.Users.Get()
                .SingleOrDefault(x => x.UserName == userName);

            if (user == null)
            {
                return null;
            }

            var rawNutritionistAlliments = UnitOfWork.Aliments.Get()
                .Where(x => x.IdNutritionist == user.IdUser);

            var rawNutritionistRecipes = UnitOfWork.Recipes.Get()
                .Where(x => x.IdNutritionist == user.IdUser);


            var details = new DetailsNutritionist()
            {
                UserName = userName,
                Name = user.FirstName + " " + user.LastName,
                Email = user.Email,
                ContributionsTotal = rawNutritionistAlliments.Count() + rawNutritionistRecipes.Count(),
                LastRecipe = rawNutritionistRecipes.Take(3)
                    .Select(x => new RecipeModel()
                    {
                        Name = x.Name,
                        IdRecipe = x.IdRecipe,
                        Time = x.Time
                    })
                    .ToList(),
                LastAliments = rawNutritionistAlliments.Take(3)
                    .Select(x => Mapper.Map<Aliment, DetailAlimentModel>(x))
                    .ToList()
            };

            return details;
        }


        public void RegisterRegularUser(RegularRegisterModel model)
        {
            ExecuteInTransaction(uow =>
            {
                RegisterRegularUserValidator.Validate(model).ThenThrow();

                var existingUser = uow.Users.Get().SingleOrDefault(x => x.Email == model.Email || model.UserName == x.UserName);
                model.Password = BCryptNet.HashPassword(model.Password);

                // adaug in users
                var user = Mapper.Map<RegularRegisterModel, User>(model);
                user.Alive = true;
                user.IdRole = Guid.Parse("4EA83F97-6116-44E2-B6EB-437A3BE9C12C");
                user.IdUser = Guid.NewGuid();
                var userId = user.IdUser;

                if (existingUser == null)
                {
                    uow.Users.Insert(user);
                }
                else
                {
                    existingUser.Password = user.Password;
                    existingUser.Email = user.Email;
                    existingUser.LastName = user.LastName;
                    existingUser.FirstName = user.FirstName;
                    existingUser.UserName = user.UserName;
                    existingUser.IdRole = user.IdRole;
                    existingUser.Alive = true;
                    uow.Users.Update(existingUser);

                    userId = existingUser.IdUser;
                }

                //adaug in regularUsers
                var regularUser = Mapper.Map<RegularRegisterModel, RegularUser>(model);
                regularUser.IdRegularUser = userId;
                uow.RegularUsers.Insert(regularUser);

                uow.SaveChanges();
            });
        }

        public CurrentUserDto Login(LoginModel model)
        {
            LoginValidator.Validate(model).ThenThrow();

            var checkedUser = UnitOfWork.Users.Get().FirstOrDefault(x => x.Email == model.Email);

            if (checkedUser == null || !checkedUser.Alive)
            {
                return new CurrentUserDto { IsAuthenticated = false};
            }

            var role = UnitOfWork.Roles.Get().SingleOrDefault(r => r.IdRole == checkedUser.IdRole).Name;

            return new CurrentUserDto
            {
                Id = checkedUser.IdUser,
                FirstName = checkedUser.FirstName,
                LastName = checkedUser.LastName,
                UserName = checkedUser.UserName,
                IsAuthenticated = true,
                Role = role
            };
        }

        public RegularProfileModel ProfileRegularUserById(Guid id)
        {
            var regularUser = UnitOfWork.RegularUsers.Get()
                .FirstOrDefault(x => x.IdRegularUser == id);

            if (regularUser == null)
            {
                return null;
            }

            var user = UnitOfWork.Users.Get()
                .FirstOrDefault(x => x.IdUser == id);

            return new RegularProfileModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,

                Weight = regularUser.Weight,
                Height = regularUser.Height,
                BirthDay = regularUser.BirthDate,
                GenderId = regularUser.Gender.ToString()
            };
        }

        public void EditProfile(RegularProfileModel model)
        {
            ExecuteInTransaction(uow =>
            {
                EditRegularUserValidator.Validate(model).ThenThrow();

                // verific daca userul exista si are parola buna
                var user = UnitOfWork.Users.Get().FirstOrDefault(x => x.Email == model.Email);

                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.UserName = model.UserName;
                    if (model.UserImage != null)
                    {
                        user.UserImage = ImageConvert(model.UserImage);
                    }

                    uow.Users.Update(user);

                    var regular = UnitOfWork.RegularUsers.Get().FirstOrDefault(x => x.IdRegularUser == user.IdUser);
                    if (regular != null)
                    {
                        regular.BirthDate = model.BirthDay;
                        regular.Height = model.Height;
                        regular.Weight = model.Weight;

                        if (model.GenderId != null)
                        {
                            regular.Gender = Int32.Parse(model.GenderId);
                        }

                        uow.RegularUsers.Update(regular);
                    }
                    
                    uow.SaveChanges();
                }
            });
        }

        public void EditProfileSpecial(SpecialProfileModel model)
        {
            ExecuteInTransaction(uow =>
            {
                EditSpecialUserValidator.Validate(model).ThenThrow();

                // verific daca userul exista si are parola buna
                var user = UnitOfWork.Users.Get().FirstOrDefault(x => x.Email == model.Email);

                if (user != null)
                {
                    // Mapper.Map<User>(model);
                    //var mappedUser = Mapper.Map<SpecialProfileModel,User>(model);
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.UserName = model.UserName;
                    if(model.UserImage != null)
                    {
                        user.UserImage = ImageConvert(model.UserImage);
                    }

                    uow.Users.Update(user);

                    var specialUser = UnitOfWork.SpecialUsers.Get().FirstOrDefault(x =>  x.IdSpecialUser == user.IdUser);
                    if(specialUser != null)
                    {
                        specialUser.ShortDescription = model.ShortDescription;
                        specialUser.LongDescription = model.LongDescription;
                        uow.SpecialUsers.Update(specialUser);
                    }

                    uow.SaveChanges();
                }
            });
        }

        public byte[] GetProfilePicOrDefault(Guid userId)
        {
            var image = UnitOfWork.Users.Get()
                .SingleOrDefault(u => u.IdUser == userId)?
                .UserImage;

            if (image == null)
            {
                var path = "./wwwroot/image/no-profile.jpg";
                image = File.ReadAllBytes(path);
            }
            
            return image;
        }

        public byte[] GetProfilePicOrDefault(string userName)
        {
            var image = UnitOfWork.Users.Get()
                .SingleOrDefault(u => u.UserName == userName)?
                .UserImage;

            if (image == null)
            {
                var path = "./wwwroot/image/no-profile.jpg";
                image = File.ReadAllBytes(path);
            }

            return image;
        }

        public ChangePasswordModel GetChangePasswordModel(Guid id)
        {
            var user = UnitOfWork.Users.Get()
                .FirstOrDefault(x => x.IdUser == id);

            if (user == null)
            {
                return null;
            }

            return new ChangePasswordModel { Email = user.Email };
        }

        public void ChangePassword(ChangePasswordModel model)
        {
            ExecuteInTransaction(uow => {
                ChangePasswordValidator.Validate(model).ThenThrow();

                var user = UnitOfWork.Users.Get().FirstOrDefault(x => x.Email == model.Email);
                user.Password = BCryptNet.HashPassword(model.NewPassword);

                UnitOfWork.Users.Update(user);
                uow.SaveChanges();
            });
        }

        public LoginModel GetLoginModel(Guid id)
        {
            var user = UnitOfWork.Users.Get()
                .FirstOrDefault(x => x.IdUser == id);

            if (user == null)
            {
                return null;
            }

            return new LoginModel
            { 
                Email = user.Email,
                Role =  UnitOfWork.Roles.Get().FirstOrDefault(r => r.IdRole == user.IdRole)?.Name
            };
        }

        public SpecialProfileModel ProfileSpecialUserById(Guid id)
        {
            var specialUser = UnitOfWork.SpecialUsers.Get()
                .FirstOrDefault(x => x.IdSpecialUser == id); 
            
            if (specialUser == null)
            {
                return null;
            }

            var user = UnitOfWork.Users.Get()
                .FirstOrDefault(x => x.IdUser == id);
            
            return new SpecialProfileModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,

                ShortDescription = specialUser.ShortDescription,
                LongDescription = specialUser.LongDescription,
                Role = UnitOfWork.Roles.Get().FirstOrDefault(r => r.IdRole == user.IdRole)?.Name
            };
        }

        public void DeleteAccount(LoginModel model)
        {
            var role = UnitOfWork.Users.Get()
                .Include(a => a.IdRoleNavigation)
                .Where(a => a.Email == model.Email)
                .Select(a => a.IdRoleNavigation.Name)
                .SingleOrDefault();

            switch (role)
            {
                case "Regular":
                    DeleteRegularUser(model);
                    break;
                case "Nutritionist":
                    DeleteNutritionist(model);
                    break;
                case "Trainer":
                    DeleteTrainer(model);
                    break;
                default:
                    DeleteAdmin(model);
                    break;
            }
        }

        private void DeleteRegularUser(LoginModel model)
        {
            ExecuteInTransaction(uow =>
            {
                LoginValidator.Validate(model).ThenThrow();

                var user = uow.Users.Get().FirstOrDefault(x => x.Email == model.Email);

                DeleteRegularUser(user, uow);
            });
        }

        private void DeleteTrainer(LoginModel model)
        {
            ExecuteInTransaction(uow =>
            {
                LoginValidator.Validate(model).ThenThrow();

                var user = uow.Users.Get().FirstOrDefault(x => x.Email == model.Email);

                DeleteTrainer(user, uow);
            });
        }

        private void DeleteNutritionist(LoginModel model)
        {
            ExecuteInTransaction(uow =>
            {
                LoginValidator.Validate(model).ThenThrow();

                var user = uow.Users.Get().FirstOrDefault(x => x.Email == model.Email);

                DeleteNutritionist(user, uow);
            });
        }

        private void DeleteAdmin(LoginModel model)
        {
            ExecuteInTransaction(uow =>
            {
                LoginValidator.Validate(model).ThenThrow();

                var user = uow.Users.Get().FirstOrDefault(x => x.Email == model.Email);

                DeleteAdmin(user, uow);
            });
        }
        
        private List<SavedItemModel> GetUserSavedItems(Guid userId)
        {
            var rawList = UnitOfWork.Saved.Get()
                .Include(x => x.IdRecipeNavigation)
                .Include(x => x.IdWorkoutNavigation)
                .Where(x => x.IdRegularUser == userId)
                .OrderByDescending(x => x.Date)
                .Take(3)
                .ToList();

            var List = new List<SavedItemModel>();

            rawList.ForEach(x => {
                var saved = new SavedItemModel();
                saved.IsWorkout = x.IdRecipeNavigation == null;
                saved.IdSavedItem = (x.IdRecipeNavigation == null) ? x.IdWorkout : x.IdRecipe;
                saved.Name = (x.IdRecipeNavigation == null) ? x.IdWorkoutNavigation.Name : x.IdRecipeNavigation.Name;
                saved.Link = (x.IdRecipeNavigation == null) ? x.IdWorkoutNavigation.LinkUrl : null;

                List.Add(saved);
            });

            return List;
        }

        private List<FriendRecommand> GetUserSharedItems(Guid userId)
        {
            var rawList = UnitOfWork.Recommandations.Get()
                .Include(x => x.IdRecipeNavigation)
                .Include(x => x.IdWorkoutNavigation)
                .Where(x => x.IdFromUser == userId)
                .OrderByDescending(x => x.SendDate)
                .Take(3)
                .ToList();

            var List = new List<FriendRecommand>();

            rawList.ForEach(x => {
                var saved = new FriendRecommand();
                saved.IsWorkout = x.IdRecipeNavigation == null;
                saved.ItemId = (Guid)((x.IdRecipeNavigation == null) ? x.IdWorkout : x.IdRecipe);
                saved.Name = (x.IdRecipeNavigation == null) ? x.IdWorkoutNavigation.Name : x.IdRecipeNavigation.Name;
                saved.Link = (x.IdRecipeNavigation == null) ? x.IdWorkoutNavigation.LinkUrl : null;

                List.Add(saved);
            });

            return List;
        }

        public string ForgotPassword(ForgotPasswordModel model)
        {
            ExecuteInTransaction(uow =>
            {
                ForgotPasswordValidator.Validate(model).ThenThrow(model);

                var newToken = Guid.NewGuid();

                var existingToken = uow.EmailTokens.Get().SingleOrDefault(e => e.Email == model.Email);

                if(existingToken == null)
                {
                    existingToken = new EmailTokens()
                    {
                        Email = model.Email,
                        Date = DateTime.Now,
                        Token = newToken,
                    };

                    uow.EmailTokens.Insert(existingToken);
                } 
                else
                {
                    //var time = DateTime.Now.Subtract(existingToken.Date).TotalSeconds;

                    existingToken.Token = newToken; 
                    existingToken.Date = DateTime.Now;

                    uow.EmailTokens.Update(existingToken);
                }

                uow.SaveChanges();

                return newToken.ToString();
            });
            return null;
        }

        public ResetForgotPasswordModel ResetForgotPassword(string email)
        {
            var model = ExecuteInTransaction(uow =>
            {
                var existingToken = uow.EmailTokens.Get()
                    .SingleOrDefault(e => e.Email == email)?
                    .Token.ToString();

                var user = UnitOfWork.Users.Get()
                    .FirstOrDefault(x => x.Email == email);
                user.Password = BCryptNet.HashPassword(existingToken);

                UnitOfWork.Users.Update(user);
                uow.SaveChanges();

                return new ResetForgotPasswordModel() 
                {
                    Email = email, 
                    Password = existingToken 
                };
            });

            return model;
        }
    }
}
