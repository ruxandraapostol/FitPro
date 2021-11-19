using FitPro.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitPro.BusinessLogic
{
    public class CreateProgramModel
    {
        public int TimePeriod { get; set; }
        public List<Guid> SelectedCategories { get; set; }
        public List<ListItemModel<string, Guid>> Categories { get; set; }
    }
}
