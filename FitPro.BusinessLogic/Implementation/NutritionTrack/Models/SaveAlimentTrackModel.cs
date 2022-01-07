using FitPro.Common;
using System;
using System.Collections.Generic;

namespace FitPro.BusinessLogic
{
    public class SaveAlimentTrackModel
    {
        public Guid IdAliment { get; set; } 

        public string AlimentName { get; set; }
        public int Quantity { get; set; }

        public DateTime Date { get; set; }

        public Guid IdRegularUser { get; set; }

        public List<ListItemModel<string, Guid?>> FoodList { get; set; }
    }
}
