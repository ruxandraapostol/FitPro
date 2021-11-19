using FitPro.Common;
using System;
using System.Collections.Generic;

#nullable disable

namespace FitPro.Entities
{
    public partial class Recommandation : IEntity
    {
        public Guid IdRecommandation { get; set; }
        public Guid IdFromUser { get; set; }
        public Guid IdToUser { get; set; }
        public Guid? IdWorkout { get; set; }
        public Guid? IdRecipe { get; set; }
        public DateTime SendDate { get; set; }
        public string Comment { get; set; }

        public virtual RegularUser IdFromUserNavigation { get; set; }
        public virtual Recipe IdRecipeNavigation { get; set; }
        public virtual RegularUser IdToUserNavigation { get; set; }
        public virtual Workout IdWorkoutNavigation { get; set; }
    }
}
