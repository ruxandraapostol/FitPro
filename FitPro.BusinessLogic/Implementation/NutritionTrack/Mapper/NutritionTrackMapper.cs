using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FitPro.Entities;

namespace FitPro.BusinessLogic
{
    public class NutritionTrackMapper : Profile
    {
        public NutritionTrackMapper()
        {
            CreateMap<SaveAlimentTrackModel, AlimentRegularUser>();
            CreateMap<AlimentRegularUser, SaveAlimentTrackModel>()
                .ForMember(src => src.AlimentName, opt => opt.MapFrom(dest => dest.IdAlimentNavigation.Name));
        }
    }
}
