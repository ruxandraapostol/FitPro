using FitPro.Common;
using System;
using System.Collections.Generic;

#nullable disable

namespace FitPro.Entities
{
    public partial class FriendsList : IEntity
    {
        public Guid IdUser1 { get; set; }
        public Guid IdUser2 { get; set; }

        public virtual RegularUser IdUser1Navigation { get; set; }
        public virtual RegularUser IdUser2Navigation { get; set; }
    }
}
