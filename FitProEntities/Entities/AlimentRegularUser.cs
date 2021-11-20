using FitPro.Common;
using System;

namespace FitPro.Entities
{
    public partial class AlimentRegularUser : IEntity
    {
        public Guid IdAliment { get; set; }
        public Guid IdRegularUser { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }

        public virtual Aliment IdAlimentNavigation { get; set; }
        public virtual RegularUser IdRegularUserNavigation { get; set; }
    }
}
