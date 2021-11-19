using System.Collections.Generic;
using System;

namespace FitPro.BusinessLogic
{
    public class FitProProgramModel
    {
        public Guid ProgramId { get; set; }
        public List<ProgramWorkoutModel> WorkoutsList { get; set; }
        public DateTime StartDay { get; set; }
        public int CurrentDay { get; set; }
        public string LinkUrl { get; set; }
        public int TotalDays { get; set; }
        public List<String> Categories { get; set; }
    }
}
