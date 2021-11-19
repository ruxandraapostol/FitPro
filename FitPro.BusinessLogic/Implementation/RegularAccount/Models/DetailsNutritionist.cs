using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitPro.BusinessLogic
{
    public class  DetailsNutritionist
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int ContributionsTotal { get; set; }
        public List<RecipeModel> LastRecipe { get; set; }
        public List<DetailAlimentModel> LastAliments { get; set; }
    }
}
