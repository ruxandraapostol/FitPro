using FitPro.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitPro.BusinessLogic
{
    public class AdminAddUserModel
    {
        public Guid IdAdmin { get; set; }

        [Required(ErrorMessage = "The email is mandatory.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The username is mandatory.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "The first name is mandatory.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The last name is mandatory.")]
        public string LastName { get; set; }

        public Guid IdRole { get; set; }

        [Required(ErrorMessage = "The new user password is mandatory.")]
        [DataType(DataType.Password)]
        public string RegisterPassword { get; set; }

        [Required(ErrorMessage = "The new user confirm password is mandatory.")]
        [DataType(DataType.Password)]
        [Compare("RegisterPassword", ErrorMessage = "The password and the confirm password must be the same.")]
        public string RegisterConfirmPassword { get; set; }

        [Required(ErrorMessage = "Your password is mandatory.")]
        [DataType(DataType.Password)]
        public string AdminPassword { get; set; }

        public List<ListItemModel<string, Guid>> Roles;
        
    }
}
