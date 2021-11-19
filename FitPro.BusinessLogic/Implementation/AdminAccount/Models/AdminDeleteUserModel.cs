using System;
using System.ComponentModel.DataAnnotations;

namespace FitPro.BusinessLogic
{
    public class AdminDeleteUserModel
    {
        public Guid IdAdmin { get; set; }
        public Guid IdUser { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }

        [Required(ErrorMessage = "The admin password is mandatory.")]
        [DataType(DataType.Password)]
        public string AdminPassword { get; set; }
    }
}
