using FitPro.Common;
using System;
using System.Collections.Generic;

#nullable disable

namespace FitPro.Entities
{
    public partial class FitProProgram : IEntity
    {
        public FitProProgram()
        {
            FitProProgramCategories = new HashSet<FitProProgramCategory>();
            FitProProgramWorkouts = new HashSet<FitProProgramWorkout>();
            RegularUserFitProPrograms = new HashSet<RegularUserFitProProgram>();
        }

        public Guid IdProgram { get; set; }
        public int TimePeriod { get; set; }

        public virtual ICollection<FitProProgramCategory> FitProProgramCategories { get; set; }
        public virtual ICollection<FitProProgramWorkout> FitProProgramWorkouts { get; set; }
        public virtual ICollection<RegularUserFitProProgram> RegularUserFitProPrograms { get; set; }
    }
}
