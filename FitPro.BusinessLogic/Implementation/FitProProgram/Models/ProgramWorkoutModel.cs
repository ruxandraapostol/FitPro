using System;

namespace FitPro.BusinessLogic
{
    public class ProgramWorkoutModel
    {
        public Guid IdWorkout { get; set; }
        public string Name { get; set; }
        public string LinkUrl { get; set; }
        public int Day { get; set; }
    }
}
