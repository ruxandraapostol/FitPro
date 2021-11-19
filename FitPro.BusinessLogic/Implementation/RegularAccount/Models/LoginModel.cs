using System.ComponentModel.DataAnnotations;

namespace FitPro.BusinessLogic
{
    public class LoginModel
    {
        [Required(ErrorMessage = "The email is mandatory.")]
        public string Email { get; set; }


        [Required(ErrorMessage = "The password is mandatory.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Role { get; set; } 
    }
}
