using FitPro.Common;
using FitPro.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using BCryptNet = BCrypt.Net.BCrypt;

namespace FitPro.BusinessLogic
{
    public class AdminService : BaseService
    {
        private readonly AdminAddUserValidation AddUserValidator;
        private readonly AdminDeleteUserValidation DeleteUserValidator;
        public AdminService(ServiceDependencies serviceDependencies) : base(serviceDependencies)
        {
            AddUserValidator = new AdminAddUserValidation(this);
            DeleteUserValidator = new AdminDeleteUserValidation(this);
        }

        public UsersListModel GetUsers(int currentPage, int rowNumber, string sortColumn, int columnSortByIndex, string searchString)
        { 
            var model = new UsersListModel(rowNumber, currentPage, sortColumn, columnSortByIndex, searchString);

            var list = UnitOfWork.Users.Get()
                .Where(u => u.Alive == true)
                .Select(u => new AdminUserModel()
                {
                    IdUser = u.IdUser,
                    UserName = u.UserName,
                    Name = u.FirstName + " " + u.LastName,
                    Email = u.Email,
                    Role = u.IdRoleNavigation.Name
                });

            if (searchString!= null) {
                var lowerSearchString = searchString.ToLower();
                list = list.Where(u => u.UserName.ToLower().Contains(lowerSearchString) || u.Name.ToLower().Contains(lowerSearchString)
                 || u.Email.ToLower().Contains(lowerSearchString) || u.Role.ToLower().Contains(lowerSearchString));
            }

            var sortList = GetUsersOrdered(model, list);

            if(rowNumber < 0)
            {
                rowNumber = 15;
            }

            model.UsersList = sortList
                .Skip(rowNumber * (currentPage - 1))
                .Take(rowNumber)
                .ToList();

            model.TotalNumberUsers = list.Count();
            
            return model;
        }
       
        public void PopulateRolesDropDown(AdminAddUserModel model)
        {
            model.Roles = UnitOfWork.Roles.Get()
                .Select(x => new ListItemModel<string, Guid>()
                {
                    Text = x.Name,
                    Value = x.IdRole
                }).ToList();
        }

        public void AdminAddUser(AdminAddUserModel model)
        {
            ExecuteInTransaction(uow =>
            {
                AddUserValidator.Validate(model).ThenThrow(model);

                var existingUser = uow.Users.Get().SingleOrDefault(x => x.Email == model.Email || model.UserName == x.UserName);
                model.RegisterPassword = BCryptNet.HashPassword(model.RegisterPassword);

                // adaug in users
                var user = Mapper.Map<AdminAddUserModel, User>(model);
                user.IdUser = Guid.NewGuid();
                user.Alive = true;
                user.IdRole = model.IdRole;
                var userId = user.IdUser;

                if(existingUser == null)
                {
                    uow.Users.Insert(user);
                } else
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


                if(uow.Roles.Get().SingleOrDefault(x => x.IdRole == user.IdRole)?.Name == "Regular")
                {
                    //adaug in regularusers
                    var regularUser = new RegularUser();
                    regularUser.IdRegularUser = userId;
                    uow.RegularUsers.Insert(regularUser);
                } else
                {
                    var specialUser = new SpecialUser();
                        specialUser.IdSpecialUser = userId;
                    uow.SpecialUsers.Insert(specialUser);
                }

                uow.SaveChanges();
            });
        }

        public AdminDeleteUserModel GetDeleteUserModel(Guid idAdmin, string email)
        {
            var model = new AdminDeleteUserModel();
            model.IdAdmin = idAdmin;

            var user = UnitOfWork.Users.Get().SingleOrDefault(u => u.Email == email);
            model = Mapper.Map<User, AdminDeleteUserModel>(user);
            model.Role = UnitOfWork.Roles.Get().SingleOrDefault(r => r.IdRole == user.IdRole).Name;

            return model;
        }

        public void AdminDeleteUser(AdminDeleteUserModel model)
        {
            switch (model.Role)
            {
                case "Regular":
                    AdminDeleteRegularUser(model);
                    break;
                case "Nutritionist":
                    AdminDeleteNutritionist(model);
                    break;
                case "Trainer":
                    AdminDeleteTrainer(model);
                    break;
                default:
                    AdminDeleteAdmin(model);
                    break;
            }
        }

        private void AdminDeleteRegularUser(AdminDeleteUserModel model)
        {
            ExecuteInTransaction(uow =>
            {
                DeleteUserValidator.Validate(model).ThenThrow();

                var user = uow.Users.Get().FirstOrDefault(x => x.Email == model.Email);

                DeleteRegularUser(user, uow);
            });
        }

        private void AdminDeleteTrainer(AdminDeleteUserModel model)
        {
            ExecuteInTransaction(uow =>
            {
                DeleteUserValidator.Validate(model).ThenThrow();

                var user = uow.Users.Get().FirstOrDefault(x => x.Email == model.Email);

                DeleteTrainer(user, uow);
            });
        }

        private void AdminDeleteNutritionist(AdminDeleteUserModel model)
        {
            ExecuteInTransaction(uow =>
            {
                DeleteUserValidator.Validate(model).ThenThrow();

                var user = uow.Users.Get().FirstOrDefault(x => x.Email == model.Email);

                DeleteNutritionist(user, uow);
            });
        }

        private void AdminDeleteAdmin(AdminDeleteUserModel model)
        {
            ExecuteInTransaction(uow =>
            {
                DeleteUserValidator.Validate(model).ThenThrow();

                var user = uow.Users.Get().FirstOrDefault(x => x.Email == model.Email);

                DeleteAdmin(user, uow);
            });
        }

        private IQueryable<AdminUserModel> GetUsersOrdered(UsersListModel model, IQueryable<AdminUserModel> list)
        {
            switch(model.SortColumn)
            {
                case "Email":
                    return (model.SortColumnIndex < 0) ? list.OrderByDescending(x => x.Email) : list.OrderBy(x => x.Email);
                case "UserName":
                    return (model.SortColumnIndex < 0) ? list.OrderByDescending(x => x.UserName) : list.OrderBy(x => x.UserName);
                case "Name":
                    return (model.SortColumnIndex < 0) ? list.OrderByDescending(x => x.Name) : list.OrderBy(x => x.Name);
                case "Role":
                    return (model.SortColumnIndex < 0) ? list.OrderByDescending(x => x.Role) : list.OrderBy(x => x.Role);
                default:
                    return list;
            }
        }

    }
}
