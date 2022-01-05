using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitPro.Entities
{
    public class DropDowns
    {
        public List<SelectListItem> Genders;
        public List<SelectListItem> Macronutrients;

        public DropDowns()
        {
            this.Genders =  Enum.GetValues(typeof(Gender))
                .Cast<Gender>()
                .Select(x => new SelectListItem
                {
                    Text = x.ToString(),
                    Value = ((int)x).ToString()
                })
                .ToList();

            this.Macronutrients = Enum.GetValues(typeof(Macronutrients))
                .Cast<Macronutrients>()
                .Select(x => new SelectListItem
                {
                    Text = x.ToString(),
                    Value = ((int)x).ToString()
                })
                .ToList();
        }
    }
}
