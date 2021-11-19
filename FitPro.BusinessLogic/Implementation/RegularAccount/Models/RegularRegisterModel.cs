using FitPro.Common;
using FitPro.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FitPro.BusinessLogic
{
    public class RegularRegisterModel
    {
        [Required(ErrorMessage = "The email is mandatory.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The username is mandatory.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "The password is mandatory.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "The confirm password is mandatory.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and the confirm password must be the same.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "The first name is mandatory.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "The last name is mandatory.")]
        public string LastName { get; set; }

        public DateTime? BirthDay { get; set; }
        public int? Weight { get; set; }
        public int? Height { get; set; }

        public int? GenderId { get; set; }
        public int Streak { get; set; } 
    }
}
