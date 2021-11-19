using FitPro.Common;
using System;
using System.Collections.Generic;


namespace FitPro.Entities
{
    public partial class Aliment : IEntity
    {
        public Aliment()
        {
            AlimentRegularUsers = new HashSet<AlimentRegularUser>();
        }

        public Guid IdAliment { get; set; }
        public string Name { get; set; }
        public double Calories { get; set; }
        public double Fat { get; set; }
        public double Carbo { get; set; }
        public double Protein { get; set; }

        public Guid IdNutritionist { get; set; }

        public virtual SpecialUser IdNutritionistNavigation { get; set; }

        public virtual ICollection<AlimentRegularUser> AlimentRegularUsers { get; set; }
    }
}
