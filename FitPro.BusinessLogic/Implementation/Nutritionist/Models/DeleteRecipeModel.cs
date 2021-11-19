using System;
using System.ComponentModel.DataAnnotations;

namespace FitPro.BusinessLogic
{
    public class DeleteRecipeModel
    {
        public Guid IdRecipe { get; set; }
        public string Name { get; set; }
        public Guid IdNutritionist { get; set; }
        public string NutritionistName { get; set; }

        [Required(ErrorMessage = "The Trainer Password is mandatory.")]
        [DataType(DataType.Password)]
        public string NutritionistPassword { get; set; }
    }
}
