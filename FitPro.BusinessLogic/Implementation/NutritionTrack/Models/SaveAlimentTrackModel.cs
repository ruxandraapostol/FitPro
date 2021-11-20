using System;

namespace FitPro.BusinessLogic
{
    public class SaveAlimentTrackModel
    {
        public Guid IdAliment { get; set; } 

        public int Quantity { get; set; }

        public DateTime Date { get; set; }

        public Guid IdRegularUser { get; set; }
    }
}
