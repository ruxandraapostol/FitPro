using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitPro.BusinessLogic
{
    public class RecipeListModel
    {
        public List<RecipeModel> RecipeList { get; set; }

        public FilterRecipeModel Filter { get; set; }

        public RecipeListModel()
        {
            Filter = new FilterRecipeModel();
        }
    }
}
