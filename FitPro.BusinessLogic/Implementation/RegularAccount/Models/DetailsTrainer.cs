using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitPro.BusinessLogic
{
    public class DetailsTrainer
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int ContributionsTotal { get; set; }
        public List<WorkoutModel> LastWorkouts { get; set; }
    }
}
