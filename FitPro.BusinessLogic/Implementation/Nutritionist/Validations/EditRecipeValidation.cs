using FluentValidation;

namespace FitPro.BusinessLogic
{
    public class EditRecipeValidation : AbstractValidator<EditRecipeModel>
    {
        public EditRecipeValidation(NutritionistService service)
        {
            RuleFor(u => u.AlimentsList)
                .NotEmpty().WithMessage("The Ingrediants field is required");

            RuleFor(u => u.Preparation)
                .NotEmpty().WithMessage("The Preparation field is required");

            RuleFor(u => u.Time)
                .NotEmpty().WithMessage("The Time field is required")
                .Must(u => u > 0).WithMessage("A recipe cannot take you back in the past. Please introduce a pozitive number for the recipe required time.")
                .Must(u => u < 200).WithMessage("A recipe should not take more than 3 hours. Please do not consider the resting time between the coocking session.");

            RuleFor(u => u.Calories)
                .NotEmpty().WithMessage("The Number of Calories field is required")
                .Must(u => u > 0).WithMessage("A recipe gives you energy(calories). Please introduce a positive number for the calories in a recipe")
                .Must(u => u < 1000).WithMessage("A recipe should be teasty and balanced. For this reason we consider that the required number of calories per recipe should not be greater than 2000.");


            RuleFor(u => u.NutritionistPassword)
                .NotEmpty().WithMessage("The Trainer Password field is required");


            When(r => r.NutritionistPassword != null, () =>
            {
                RuleFor(r => new { r.IdNutritionist, r.NutritionistPassword })
               .Must(x => service.CheckMatchPassword(x.IdNutritionist, x.NutritionistPassword)).WithMessage("Password is incorrect.");
            });
        }
    }
}
