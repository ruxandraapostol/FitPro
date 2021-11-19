using System;
using System.Collections.Generic;

namespace FitPro.BusinessLogic
{
    public class RecommandItemModel
    {
        public List<FriendModel> FriendsList { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public bool IsWorkout { get; set; }

        public string Comment { get; set; }
        public Guid IdItem { get; set; }
        public Guid CurrentUserId { get; set; }

        public string FromPage { get; set; }
    }
}
