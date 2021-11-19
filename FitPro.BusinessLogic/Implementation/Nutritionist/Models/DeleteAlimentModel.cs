using System;
using System.ComponentModel.DataAnnotations;

namespace FitPro.BusinessLogic
{
    public class DeleteAlimentModel
    {
        public Guid IdNutritionist { get; set; }
        public string Name { get; set; }

        [Required(ErrorMessage = "The Trainer Password is mandatory.")]
        [DataType(DataType.Password)]
        public string NutritionistPassword { get; set; }
    }
}
