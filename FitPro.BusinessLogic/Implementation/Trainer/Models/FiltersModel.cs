using FitPro.Common;
using System;
using System.Collections.Generic;

namespace FitPro.BusinessLogic
{
    public class FiltersModel
    {
        public string SortColumn { get; set; }
        public string SortColumnIndex { get; set; }
        public string SearchString { get; set; }
        public int? LowerTimeLimit { get; set; }
        public int? UpperTimeLimit { get; set; }
        public int? LowerCaloriesLimit { get; set; }
        public int? UpperCaloriesLimit { get; set; }
        public List<Guid> SelectedCategories { get; set; }
        public List<ListItemModel<string, Guid>> Categories { get; set; }
        public List<Guid> SelectedTrainers { get; set; }
        public List<ListItemModel<string, Guid>> Trainers { get; set; }

    }
}
