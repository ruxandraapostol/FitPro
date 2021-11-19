using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FitPro.BusinessLogic
{
    public class SpecialProfileModel
    {
        [Required(ErrorMessage = "The email is mandatory.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The username is mandatory.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "The first name is mandatory.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The last name is mandatory.")]
        public string LastName { get; set; }

        [StringLength(maximumLength:200, ErrorMessage = "The short description length should not be more than 200 charcters.")]
        public string ShortDescription { get; set; }

        public string LongDescription { get; set; }

        public string Role { get; set; }

        public IFormFile UserImage { get; set; }

        [Required(ErrorMessage = "The password is mandatory for editing your profile.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
