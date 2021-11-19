using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace FitPro.BusinessLogic
{
    public class DetailRecipeModel
    {
        public Guid IdRecipe { get; set; }
        public string Name { get; set; }
        public int Time { get; set; }
        public int Calories { get; set; }

        public string NutritionistName { get; set; }
        public string CategoryName { get; set; }
        public List<string> IngredientsList { get; set; }
        public List<string> PreparationList { get; set; }
        public bool FromSaved { get; set; }
        public bool FromShare { get; set; }
    }
}
