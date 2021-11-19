using System;
using System.ComponentModel.DataAnnotations;

namespace FitPro.BusinessLogic
{
    public class WorkoutModel
    {
        [Required(ErrorMessage = "The name is mandatory.")]
        public string Name { get; set; }

        public Guid IdWorkout { get; set; }

        [Required(ErrorMessage = "The Video Link is mandatory.")]
        [Url(ErrorMessage = "Please introduce a valid Url Link.")]
        public string LinkUrl { get; set; }

        public int Time { get; set; }
        public int Calories { get; set; }
        public Guid Trainer { get; set; }

        public bool IsSaved { get; set; }
    }
}
