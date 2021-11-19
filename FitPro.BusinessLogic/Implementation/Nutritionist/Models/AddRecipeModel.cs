using FitPro.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitPro.BusinessLogic
{
    public class AddRecipeModel
    {
        [Display(Name ="Name")]
        public string Name { get; set; }
        public string AlimentsList { get; set; }
        public string Preparation { get; set; }
        public int Time { get; set; }
        public int Calories { get; set; }
        public IFormFile Image { get; set; }
        public Guid IdNutritionist { get; set; }

        [Display(Name = "Category")]
        public Guid? IdCategory { get; set; }
        public List<ListItemModel<string, Guid?>> Categories { get; set; }

        [Required(ErrorMessage = "The Trainer Password is mandatory.")]
        [DataType(DataType.Password)]
        public string NutritionistPassword { get; set; }
    }
}
