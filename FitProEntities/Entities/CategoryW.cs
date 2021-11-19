using FitPro.Common;
using System;
using System.Collections.Generic;

#nullable disable

namespace FitPro.Entities
{
    public partial class CategoryW : IEntity
    {
        public CategoryW()
        {
            FitProProgramCategories = new HashSet<FitProProgramCategory>();
            WorkoutCategories = new HashSet<WorkoutCategory>();
        }

        public Guid IdCategory { get; set; }
        public string Name { get; set; }

        public virtual ICollection<FitProProgramCategory> FitProProgramCategories { get; set; }
        public virtual ICollection<WorkoutCategory> WorkoutCategories { get; set; }
    }
}
