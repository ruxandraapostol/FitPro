using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitPro.BusinessLogic
{
    public class GraphModel
    {
        public List<string> XValues { get; set; }
        public List<double> YValues { get; set; }

        public GraphModel()
        {
            XValues = new List<string>();
            YValues = new List<double>();
        }
    }
}
