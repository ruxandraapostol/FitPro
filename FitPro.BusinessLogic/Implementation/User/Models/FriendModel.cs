using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitPro.BusinessLogic
{
    public class FriendModel
    {
        public Guid IdUser { get; set; }
        public string UserName { get; set; }
        public int Streak { get; set; }
    }
}
