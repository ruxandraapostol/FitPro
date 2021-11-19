using System;

namespace FitPro.BusinessLogic
{
    public class RecipeModel
    {
        public bool IsSaved { get; set; }
        public Guid IdRecipe { get; set; }
        public string Name { get; set; }
        public int Time { get; set; }
    }
}
