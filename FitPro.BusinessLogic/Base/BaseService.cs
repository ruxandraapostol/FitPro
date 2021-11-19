using AutoMapper;
using FitPro.Common.DTOs;
using FitPro.DataAccess;
using FitPro.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Transactions;
using BCryptNet = BCrypt.Net.BCrypt;

namespace FitPro.BusinessLogic
{
    public class BaseService
    {
        protected readonly IMapper Mapper;
        protected readonly UnitOfWork UnitOfWork;
        protected readonly CurrentUserDto CurrentUser;

        public BaseService(ServiceDependencies serviceDependencies)
        {
            Mapper = serviceDependencies.Mapper;
            UnitOfWork = serviceDependencies.UnitOfWork;
            CurrentUser = serviceDependencies.CurrentUser;
        }

        protected TResult ExecuteInTransaction<TResult>(Func<UnitOfWork, TResult> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            using (var transactionScope = new TransactionScope())
            {
                var result = func(UnitOfWork);

                transactionScope.Complete();

                return result;
            }
        }

        protected void ExecuteInTransaction(Action<UnitOfWork> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            using (var transactionScope = new TransactionScope())
            {
                action(UnitOfWork);

                transactionScope.Complete();
            }
        }

        public bool NotAlreadyExistEmail(string email)
        {
            var user = UnitOfWork.Users.Get()
                .SingleOrDefault(u => email == u.Email && u.Alive);

            if(user == null)
            {
                return true;
            } else
            {
                return false;
            }
        }

        public bool CheckLinkNotAlreadyUsed(string linkUrl)
        {
            return UnitOfWork.Workouts.Get()
                .FirstOrDefault(x => x.LinkUrl == linkUrl) == null;
        }

        public bool NotAlreadyExistUsername(string username, string email)
        {
            if(username == null || email == null)
            {
                return false;
            }

            var user = UnitOfWork.Users.Get()
                .SingleOrDefault(u => (username == u.UserName && email != u.Email) && u.Alive);

            if (user == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool NotAlreadyExistUsername(string username)
        {
            var user = UnitOfWork.Users.Get()
                .SingleOrDefault(u => username == u.UserName && u.Alive);

            if (user == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckMatchPasswordEmail(string email, string password)
        {
            var hashPassword = BCryptNet.HashPassword(password);

            var user = UnitOfWork.Users.Get()
                .FirstOrDefault(u => u.Email == email);

            if (user == null || !BCryptNet.Verify(password, user?.Password))
            {
                return false;
            }

            return true;
        }

        public bool CheckMatchPassword(Guid userId, string password)
        {
            var hashPassword = BCryptNet.HashPassword(password);

            var user = UnitOfWork.Users.Get()
                .FirstOrDefault(u => u.IdUser == userId);

            if (user == null || !BCryptNet.Verify(password, user?.Password))
            {
                return false;
            }

            return true;
        }

        public bool PasswordHasNumber(string password)
        {
            return password.Any(c => char.IsDigit(c));
        }

        public bool PasswordHasUpperLetter(string password)
        {
            return password.Any(c => char.IsUpper(c));
        }

        public bool PasswordHasLowerLetter(string password)
        {
            return password.Any(c => char.IsLower(c));
        }

        public bool PasswordHasSpecialLetter(string password)
        {
            return password.Any(c => !char.IsLetterOrDigit(c));
        }


        public void DeleteRegularUser(User user, UnitOfWork uow)
        {
            if (user != null)
            {
                var listRegularUserAliment = uow.AlimentRegularUsers.Get().Where(x => x.IdRegularUser == user.IdUser);
                foreach (var elem in listRegularUserAliment)
                {
                    uow.AlimentRegularUsers.Delete(elem);
                }

                var listRegularUserPrograms = uow.RegularUserFitProPrograms.Get().Where(x => x.IdRegularUser == user.IdUser);
                foreach (var elem in listRegularUserPrograms)
                {
                    uow.RegularUserFitProPrograms.Delete(elem);
                }

                var listRequest = uow.Requests.Get().Where(x => x.IdFromUser == user.IdUser || x.IdToUser == user.IdUser);
                foreach (var elem in listRequest)
                {
                    uow.Requests.Delete(elem);
                }

                var listFriends = uow.FriendsLists.Get().Where(x => x.IdUser1 == user.IdUser || x.IdUser2 == user.IdUser);
                foreach (var elem in listRequest)
                {
                    uow.Requests.Delete(elem);
                }

                var regularUser = uow.RegularUsers.Get().FirstOrDefault(x => x.IdRegularUser == user.IdUser);
                uow.RegularUsers.Delete(regularUser);

                user.Alive = false;
                uow.Users.Update(user);

                uow.SaveChanges();
            }
        }

        public void DeleteNutritionist(User user, UnitOfWork uow)
        {

            if (user != null)
            {
                var listAliments = uow.Aliments.Get().Where(x => x.IdNutritionist == user.IdUser);
                foreach (var elem in listAliments)
                {
                    elem.IdNutritionist = Guid.Empty;
                    uow.Aliments.Update(elem);
                }

                var listRecipes = uow.Recipes.Get().Where(x => x.IdNutritionist == user.IdUser);
                foreach (var elem in listRecipes)
                {
                    elem.IdNutritionist = Guid.Empty;
                    uow.Recipes.Update(elem);
                }

                var specialUser = uow.SpecialUsers.Get().FirstOrDefault(x => x.IdSpecialUser == user.IdUser);
                uow.SpecialUsers.Delete(specialUser);

                user.Alive = false;
                uow.Users.Update(user);
                uow.SaveChanges();
            }
        }

        public void DeleteTrainer(User user, UnitOfWork uow)
        {
            if (user != null)
            {
                var listWorkouts = uow.Workouts.Get().Where(x => x.IdTrainer == user.IdUser);
                foreach (var elem in listWorkouts)
                {
                    elem.IdTrainer = Guid.Empty;
                    uow.Workouts.Update(elem);
                }

                var specialUser = uow.SpecialUsers.Get().FirstOrDefault(x => x.IdSpecialUser == user.IdUser);
                uow.SpecialUsers.Delete(specialUser);

                user.Alive = false;
                uow.Users.Update(user);
                uow.SaveChanges();
            }
        }

        public void DeleteAdmin(User user, UnitOfWork uow)
        {
            if (user != null)
            {
                var specialUser = uow.SpecialUsers.Get().FirstOrDefault(x => x.IdSpecialUser == user.IdUser);
                uow.SpecialUsers.Delete(specialUser);

                user.Alive = false;
                uow.Users.Update(user);
                uow.SaveChanges();
            }
        }

        public  byte[] ImageConvert(IFormFile imageFile)
        {
            byte[] image = null;

            if (imageFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    imageFile.CopyTo(memoryStream);
                    image = memoryStream.ToArray();
                }
            }
            return image;
        }

        public Guid GetUserIdByUsername(string userName)
        {
            return UnitOfWork.Users.Get()
                .SingleOrDefault(x => x.UserName == userName)?.IdUser ?? Guid.Empty;
        }
    }
}
