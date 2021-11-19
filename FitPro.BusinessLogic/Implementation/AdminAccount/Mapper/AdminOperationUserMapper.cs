using AutoMapper;
using FitPro.Entities;

namespace FitPro.BusinessLogic
{
    class AdminOperationUserMapper : Profile
    {
         public AdminOperationUserMapper()
        {
            CreateMap<AdminAddUserModel, User>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.RegisterPassword));

            CreateMap<User, AdminDeleteUserModel>()
                .ForMember(dest => dest.Role, opt=> opt.MapFrom(src => src.IdRoleNavigation.Name));
        }
    }
}
