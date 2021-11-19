using FitPro.Common;
using System;

namespace FitPro.Entities
{
    public partial class RegularUserFitProProgram : IEntity
    {
        public Guid IdProgram { get; set; }
        public Guid IdRegularUser { get; set; }
        public DateTime StartDate { get; set; }
        public int CurrentDay { get; set; }

        public virtual FitProProgram IdProgramNavigation { get; set; }
        public virtual RegularUser IdRegularUserNavigation { get; set; }
    }
}
