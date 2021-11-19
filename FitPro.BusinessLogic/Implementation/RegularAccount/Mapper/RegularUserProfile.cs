using AutoMapper;
using FitPro.Entities;

namespace FitPro.BusinessLogic
{
    public class RegularUserProfile : Profile
    {
        public RegularUserProfile()
        {
            CreateMap<RegularRegisterModel, User>();
            CreateMap<RegularRegisterModel, RegularUser>();
        }
    }
}
