using System;
using System.Collections.Generic;

namespace FitPro.BusinessLogic
{
    public class FriendsListModel
    {
        public Guid IdUser { get; set; }
        public List<FriendModel> FriendsList { get; set; }
        public string SearchStringPossibleFriends { get; set; }
        public string SearchStringFriends { get; set; }
    }
}
