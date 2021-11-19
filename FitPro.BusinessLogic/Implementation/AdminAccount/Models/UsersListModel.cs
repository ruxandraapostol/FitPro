using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace FitPro.BusinessLogic
{
    public class UsersListModel
    {
        public List<AdminUserModel> UsersList { get; set; }
        public string SortColumn { get; set; } 
        public int SortColumnIndex { get; set; }
        public int CurrentPage { get; set; }
        public int RowNumber { get; set; }
        public int TotalNumberUsers { get; set; }
        public string SearchString { get; set; }

        public UsersListModel(int rowNumber, int currentPage, string sortColumn, int sortColumnIndex, string searchString)
        {
            SortColumn = sortColumn;
            SortColumnIndex = sortColumnIndex;
            RowNumber = rowNumber;
            CurrentPage = currentPage;
            SearchString = searchString;
        }
    }
}
