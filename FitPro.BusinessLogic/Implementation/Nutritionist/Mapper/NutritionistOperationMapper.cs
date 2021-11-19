using AutoMapper;
using FitPro.Entities;

namespace FitPro.BusinessLogic
{
    public class NutritionistOperationMapper : Profile
    {
        public NutritionistOperationMapper()
        {
            CreateMap<Aliment, DetailAlimentModel>();
            CreateMap<Aliment, EditAlimentModel>();
            CreateMap<Aliment, DeleteAlimentModel>();
            CreateMap<EditAlimentModel, Aliment>();
            CreateMap<AddAlimentModel, Aliment>();


            CreateMap<AddRecipeModel, Recipe>();
            CreateMap<Recipe, EditRecipeModel>();
            CreateMap<Recipe, DeleteRecipeModel>();
            CreateMap<Recipe, DetailRecipeModel>();
        }
    }
}
