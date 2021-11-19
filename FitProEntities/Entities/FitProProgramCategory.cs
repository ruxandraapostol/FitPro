using FitPro.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitPro.Entities
{
    public class FitProProgramCategory : IEntity
    {
        public Guid IdProgram { get; set; }
        public Guid IdCategory { get; set; }

        public virtual CategoryW IdCategoryNavigation { get; set; }
        public virtual FitProProgram IdProgramNavigation { get; set; }
    }
}
