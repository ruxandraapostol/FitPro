using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace FitPro.BusinessLogic
{
    public class RegularProfileModel
    {
        [Required(ErrorMessage = "The email is mandatory.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The username is mandatory.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "The first name is mandatory.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The last name is mandatory.")]
        public string LastName { get; set; }

        public IFormFile UserImage { get; set; }

        public DateTime? BirthDay { get; set; }
        public int? Weight { get; set; }
        public int? Height { get; set; }
        public String GenderId { get; set; }

        public int Streak { get; set; }

        [Required(ErrorMessage = "The password is mandatory for editing your profile.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
