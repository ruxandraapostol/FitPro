using FitPro.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitPro.BusinessLogic
{
    public class AddWorkoutModel
    {
        [Required(ErrorMessage = "The name is mandatory.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Description is mandatory.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "The Time is mandatory.")]
        public int Time { get; set; }

        [Required(ErrorMessage = "The Video Link is mandatory.")]
        [Url(ErrorMessage = "Please introduce a valid Url Link.")]
        public string LinkUrl { get; set; }

        [Required(ErrorMessage = "The Calories is mandatory.")]
        public int Calories { get; set; }

        public Guid IdTrainer { get; set; }

        public List<Guid> SelectedCategories { get; set; }
        public List<ListItemModel<string, Guid>> Categories { get; set; }

        [Required(ErrorMessage = "The Trainer Password is mandatory.")]
        [DataType(DataType.Password)]
        public string TrainerPassword { get; set; }
    }
}
