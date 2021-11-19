using FluentValidation;

namespace FitPro.BusinessLogic
{
    public class DeleteRecipeValidation : AbstractValidator<DeleteRecipeModel>
    {
        public DeleteRecipeValidation(NutritionistService service)
        {

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
