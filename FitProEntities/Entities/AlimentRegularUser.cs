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
        public int TotalCalories { get; set; }
        public int TotalProtein { get; set; }
        public int TotalFat { get; set; }
        public int TotalCarbo { get; set; }

        public virtual Aliment IdAlimentNavigation { get; set; }
        public virtual RegularUser IdRegularUserNavigation { get; set; }
    }
}
