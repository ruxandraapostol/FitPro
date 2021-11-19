using FitPro.Common;
using System;
using System.Collections.Generic;

#nullable disable

namespace FitPro.Entities
{
    public partial class CategoryR : IEntity
    {
        public CategoryR()
        {
            Recipes = new HashSet<Recipe>();
        }

        public Guid IdCategory { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}
