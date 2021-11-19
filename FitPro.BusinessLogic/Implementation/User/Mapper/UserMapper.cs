using AutoMapper;
using FitPro.Entities;

namespace FitPro.BusinessLogic
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<RegularUser, FriendModel>()
                .ForMember(src => src.IdUser, opt => opt.MapFrom( dest => dest.IdRegularUser))
                .ForMember(src => src.UserName, opt => opt.MapFrom(dest => dest.IdRegularUserNavigation.UserName));

            CreateMap<Request, FriendRequestModel>()
                .ForMember(src => src.IdUser, opt => opt.MapFrom(dest => dest.IdFromUser))
                .ForMember(src => src.UserName, opt => opt.MapFrom(dest => dest.IdFromUserNavigation.IdRegularUserNavigation.UserName));

            CreateMap<User, PossibleFriendModel>()
               .ForMember(src => src.Streak, opt => opt.MapFrom(dest => dest.RegularUser.Streak));
        }
    }
}
