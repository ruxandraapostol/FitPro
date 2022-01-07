using FitPro.Common;
using System;

namespace FitPro.Entities
{
    public partial class UserActiveDays : IEntity
    {
        public Guid IdRegularUser { get; set; }
        public DateTime Date { get; set; }
        public virtual RegularUser IdRegularUserNavigation { get; set; }
    }
}
