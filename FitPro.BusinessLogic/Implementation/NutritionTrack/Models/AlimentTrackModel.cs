using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitPro.BusinessLogic
{
    public class AlimentTrackModel
    {
        public DateTime Date { get; set; }
        public string AlimentName { get; set; }
        public Guid IdAliment { get; set; }
        public int Quantity { get; set; }
        public double TotalCalories { get; set; }
        public double TotalCarbs { get; set; }
        public double TotalFats { get; set; }
        public double TotalProt { get; set; }

    }
}
