using FitPro.Common;
using System;
using System.Collections.Generic;

#nullable disable

namespace FitPro.Entities
{
    public partial class Workout : IEntity
    {
        public Workout()
        {
            FitProProgramWorkouts = new HashSet<FitProProgramWorkout>();
            Recommandations = new HashSet<Recommandation>();
            WorkoutCategories = new HashSet<WorkoutCategory>();
            Saveds = new HashSet<Saved>();
        }

        public Guid IdWorkout { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Time { get; set; }
        public Guid? IdTrainer { get; set; }
        public string LinkUrl { get; set; }
        public int Calories { get; set; }
        public Guid LastModified { get; set; }

        public virtual SpecialUser IdTrainerNavigation { get; set; }
        public virtual SpecialUser IdLastModifiedNavigation { get; set; }
        public virtual ICollection<FitProProgramWorkout> FitProProgramWorkouts { get; set; }
        public virtual ICollection<Recommandation> Recommandations { get; set; }
        public virtual ICollection<WorkoutCategory> WorkoutCategories { get; set; }
        public virtual ICollection<Saved> Saveds { get; set; }
    }
}
