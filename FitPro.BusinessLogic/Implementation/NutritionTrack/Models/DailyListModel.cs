using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitPro.BusinessLogic
{
    public class DailyListModel
    {
        public DateTime Date { get; set; }
        public bool ActiveDay { get; set; }
        public List<AlimentTrackModel> AlimentTrackList { get; set; }
        public double RecommendedCalories { get; set; }
        public double RecommendedCarbs { get; set; }
        public double RecommendedFats { get; set; }
        public double RecommendedProt { get; set; }
        public double TotalCalories { get; set; }
        public double TotalCarbs { get; set; }
        public double TotalFats { get; set; }
        public double TotalProt { get; set; }
    }
}
