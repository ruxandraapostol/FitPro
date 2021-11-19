using FitPro.Common;
using System;

namespace FitPro.Entities
{
    public partial class WorkoutCategory : IEntity
    {
        public Guid IdWorkout { get; set; }
        public Guid IdCategory { get; set; }

        public virtual CategoryW IdCategoryNavigation { get; set; }
        public virtual Workout IdWorkoutNavigation { get; set; }
    }
}
