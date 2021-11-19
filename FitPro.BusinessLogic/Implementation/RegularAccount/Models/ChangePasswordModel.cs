using System.ComponentModel.DataAnnotations;

namespace FitPro.BusinessLogic
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "The email is mandatory.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The old password is mandatory.")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "The new password is mandatory.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "The confirm password is mandatory.")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The password and the confirm password must be the same.")]
        public string ConfirmPassword { get; set; }
    }
}
