using System;

namespace FitPro.BusinessLogic
{
    public class AdminUserModel
    {
        public Guid IdUser { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
    }
}
