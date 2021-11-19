using FitPro.Common;
using System;
using System.Collections.Generic;

#nullable disable

namespace FitPro.Entities
{
    public partial class FitProProgramWorkout : IEntity
    {
        public Guid IdProgram { get; set; }
        public Guid IdWorkout { get; set; }
        public int DayNumber { get; set; }

        public virtual FitProProgram IdProgramNavigation { get; set; }
        public virtual Workout IdWorkoutNavigation { get; set; }
    }
}
