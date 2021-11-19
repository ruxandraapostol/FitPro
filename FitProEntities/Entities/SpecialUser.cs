using FitPro.Common;
using System;
using System.Collections.Generic;

#nullable disable

namespace FitPro.Entities
{
    public partial class SpecialUser : IEntity
    {
        public SpecialUser()
        {
            Aliments = new HashSet<Aliment>();
            Recipes = new HashSet<Recipe>();
            Workouts = new HashSet<Workout>();
        }

        public Guid IdSpecialUser { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }

        public virtual User IdSpecialUserNavigation { get; set; }
        public virtual ICollection<Aliment> Aliments { get; set; }
        public virtual ICollection<Recipe> Recipes { get; set; }
        public virtual ICollection<Workout> Workouts { get; set; }
        public virtual ICollection<Workout> WorkoutsLastModified { get; set; }

    }
}
