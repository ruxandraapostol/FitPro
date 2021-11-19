using AutoMapper;
using FitPro.Entities;

namespace FitPro.BusinessLogic
{
    public class TrainerOperationMapper : Profile
    {
        public TrainerOperationMapper()
        {
            CreateMap<AddWorkoutModel, Workout>();
            CreateMap<Workout, DetailWorkoutModel>()
                .ForMember(src => src.VideoLinkUrl, opt => opt.MapFrom(dest => dest.LinkUrl));
            CreateMap<Workout, EditWorkoutModel>();
            CreateMap<Workout, DeleteWorkoutModel>();
            CreateMap<Workout, WorkoutModel>()
                .ForMember(src => src.Time, opt => opt.MapFrom(dest => dest.Time ?? 0))
                .ForMember(src => src.Trainer, opt => opt.MapFrom(dest => dest.IdTrainer));

        }
    }
}
