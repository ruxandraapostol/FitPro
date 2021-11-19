using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitPro.BusinessLogic
{
    public class EditAlimentModel
    {
        public string Name { get; set; }

        public double Calories { get; set; }
        public double Fat { get; set; }
        public double Carbo { get; set; }
        public double Protein { get; set; }
        public Guid Nutritionist { get; set; }


        [Required(ErrorMessage = "The Trainer Password is mandatory.")]
        [DataType(DataType.Password)]
        public string NutritionistPassword { get; set; }
    }
}
