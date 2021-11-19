using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitPro.BusinessLogic
{
    public class AlimentsListModel
    {
        public List<DetailAlimentModel> AlimentList { get; set; }

        public string  SearchString { get; set; }

        public int CurrentPage { get; set; }
    }
}
