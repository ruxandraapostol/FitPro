using System;

namespace FitPro.BusinessLogic
{
    public class DetailAlimentModel
    {
        public string Name { get; set; }

        public double Calories { get; set; }
        public double Fat { get; set; }
        public double Carbo { get; set; }
        public double Protein { get; set; }
        public Guid IdNutritionist { get; set; }
    }
}
