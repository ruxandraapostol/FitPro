using System.Collections.Generic;

namespace FitPro.BusinessLogic
{
    public class CalendarMonthModel
    {
        public string Month { get; set; }
        public int Year { get; set; }

        public List<CalendarDayModel> Days { get; set; }

    }
}
