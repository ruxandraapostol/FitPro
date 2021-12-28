using FitPro.Common;
using System;
using System.Collections.Generic;

#nullable disable

namespace FitPro.Entities
{
    public partial class RegularUser : IEntity
    {
        public RegularUser()
        {
            AlimentRegularUsers = new HashSet<AlimentRegularUser>();
            FriendsListIdUser1Navigations = new HashSet<FriendsList>();
            FriendsListIdUser2Navigations = new HashSet<FriendsList>();
            RecommandationIdFromUserNavigations = new HashSet<Recommandation>();
            RecommandationIdToUserNavigations = new HashSet<Recommandation>();
            RegularUserFitProPrograms = new HashSet<RegularUserFitProProgram>();
            RequestIdFromUserNavigations = new HashSet<Request>();
            RequestIdToUserNavigations = new HashSet<Request>();
            Saveds = new HashSet<Saved>();
            UserActiveDays = new HashSet<UserActiveDays>();
        }

        public Guid IdRegularUser { get; set; }
        public int? Height { get; set; }
        public int? Weight { get; set; }
        public int? Gender { get; set; }
        public int? Streak { get; set; }
        public DateTime? BirthDate { get; set; }

        public virtual User IdRegularUserNavigation { get; set; }
        public virtual ICollection<Saved> Saveds { get; set; }
        public virtual ICollection<UserActiveDays> UserActiveDays { get; set; }
        public virtual ICollection<AlimentRegularUser> AlimentRegularUsers { get; set; }
        public virtual ICollection<FriendsList> FriendsListIdUser1Navigations { get; set; }
        public virtual ICollection<FriendsList> FriendsListIdUser2Navigations { get; set; }
        public virtual ICollection<Recommandation> RecommandationIdFromUserNavigations { get; set; }
        public virtual ICollection<Recommandation> RecommandationIdToUserNavigations { get; set; }
        public virtual ICollection<RegularUserFitProProgram> RegularUserFitProPrograms { get; set; }
        public virtual ICollection<Request> RequestIdFromUserNavigations { get; set; }
        public virtual ICollection<Request> RequestIdToUserNavigations { get; set; }
    }
}
