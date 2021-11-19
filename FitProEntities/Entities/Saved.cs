using FitPro.Common;
using System;

namespace FitPro.Entities
{
    public partial class Saved : IEntity
    {
        public Guid IdSaved { get; set; }
        public Guid IdRegularUser { get; set; }
        public Guid? IdWorkout { get; set; }
        public Guid? IdRecipe { get; set; }
        public DateTime? Date { get; set; }

        public virtual Recipe IdRecipeNavigation { get; set; }
        public virtual RegularUser IdRegularUserNavigation { get; set; }
        public virtual Workout IdWorkoutNavigation { get; set; }
    }
}
