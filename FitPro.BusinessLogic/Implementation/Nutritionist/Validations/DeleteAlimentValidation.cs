using FluentValidation;

namespace FitPro.BusinessLogic
{
    public class DeleteAlimentValidation : AbstractValidator<DeleteAlimentModel>
    {
        public DeleteAlimentValidation(NutritionistService service)
        {
            RuleFor(u => u.NutritionistPassword)
                .NotEmpty().WithMessage("The nutritionist password field is required");


            When(r => r.NutritionistPassword != null, () =>
            {
                RuleFor(r => new { r.IdNutritionist, r.NutritionistPassword })
               .Must(x => service.CheckMatchPassword(x.IdNutritionist, x.NutritionistPassword)).WithMessage("Password is incorrect.");
            });

        }
    }
}
