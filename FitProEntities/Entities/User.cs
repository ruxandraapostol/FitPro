using FitPro.Common;
using System;

namespace FitPro.Entities
{
    public partial class User : IEntity
    {
        public Guid IdUser { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public byte[] UserImage { get; set; }
        public bool Alive { get; set; }
        public Guid IdRole { get; set; }

        public virtual Role IdRoleNavigation { get; set; }
        public virtual RegularUser RegularUser { get; set; }
        public virtual SpecialUser SpecialUser { get; set; }
    }
}
