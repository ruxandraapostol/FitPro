using System;
using System.Collections.Generic;

namespace FitPro.Common.DTOs
{
    public class CurrentUserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[] UserImage { get; set; }
        public int Streak { get; set; }
        public bool IsAuthenticated { get; set; }

        public string Role { get; set; }
    }
}
