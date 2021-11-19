using AutoMapper;
using FitPro.Entities;


namespace FitPro.BusinessLogic
{
    public class ProgramMapper : Profile
    {
        public ProgramMapper()
        {
            CreateMap<Workout, ProgramWorkoutModel>();
        }
    }
}
