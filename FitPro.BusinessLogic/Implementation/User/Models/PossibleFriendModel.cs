using System;

namespace FitPro.BusinessLogic
{
    public class PossibleFriendModel
    {
        public Guid IdUser { get; set; }
        public string UserName { get; set; }
        public int Streak { get; set; }
        public string Status { get; set; }
    }
}