using System;
using System.ComponentModel.DataAnnotations;

namespace FitPro.BusinessLogic
{
    public class DeleteWorkoutModel
    {
        public string Name { get; set; }

        public string LinkUrl { get; set; }

        public Guid IdTrainer { get; set; }

        public string Author { get; set; }

        [Required(ErrorMessage = "The Trainer Password is mandatory.")]
        [DataType(DataType.Password)]
        public string TrainerPassword { get; set; }
    }
}
