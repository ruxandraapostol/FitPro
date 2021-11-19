using System;

namespace FitPro.BusinessLogic
{
    public class FriendRecommand
    {
        public string FriendUserName {get; set;}
        public string Date { get; set; }
        public Guid ItemId { get; set; }
        public bool IsWorkout { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Comment { get; set; }
    }
}
