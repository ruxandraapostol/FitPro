using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitPro.BusinessLogic
{
    public class DetailsRegular
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int FriendsNumber { get; set; }
        public List<SavedItemModel> LastSavedItems {get; set;}
        public List<FriendRecommand> LastRecommand { get; set; }
    }
}
