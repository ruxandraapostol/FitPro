using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitPro.BusinessLogic
{
    public class SavedItemModel
    {
        public bool IsWorkout { get; set; }
        public Guid? IdSavedItem { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
    }
}
