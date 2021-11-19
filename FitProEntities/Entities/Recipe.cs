using FitPro.Common;
using System;
using System.Collections.Generic;

#nullable disable

namespace FitPro.Entities
{
    public partial class Recipe : IEntity
    {
        public Recipe()
        {
            Recommandations = new HashSet<Recommandation>();
            Saveds = new HashSet<Saved>();
        }

        public Guid IdRecipe { get; set; }
        public string Name { get; set; }
        public string AlimentsList { get; set; }
        public string Preparation { get; set; }
        public int Time { get; set; }
        public int Calories   { get; set; }
        public byte[] Image { get; set; }
        public Guid? IdCategory { get; set; }
        public Guid IdNutritionist { get; set; }

        public virtual SpecialUser IdNutritionistNavigation { get; set; }

        public virtual CategoryR IdCategoryNavigation { get; set; }

        public virtual ICollection<Saved> Saveds { get; set; }
        public virtual ICollection<Recommandation> Recommandations { get; set; }
    }
}
