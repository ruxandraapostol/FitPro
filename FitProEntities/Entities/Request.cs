using FitPro.Common;
using System;
using System.Collections.Generic;

#nullable disable

namespace FitPro.Entities
{
    public partial class Request : IEntity
    {
        public Guid IdFromUser { get; set; }
        public Guid IdToUser { get; set; }
        public int Status { get; set; }

        public DateTime Date { get; set; }
        public virtual RegularUser IdFromUserNavigation { get; set; }
        public virtual RegularUser IdToUserNavigation { get; set; }
    }
}
