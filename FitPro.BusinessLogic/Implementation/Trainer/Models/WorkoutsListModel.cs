using System.Collections.Generic;

namespace FitPro.BusinessLogic
{
    public class WorkoutsListModel
    {
        public List<WorkoutModel> WorkoutsList { get; set; }

        public FiltersModel FilterMore { get; set; }

        public WorkoutsListModel()
        {
            FilterMore = new FiltersModel();
        }
    }
}
