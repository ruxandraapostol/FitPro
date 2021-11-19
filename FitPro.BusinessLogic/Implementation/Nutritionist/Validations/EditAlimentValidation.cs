using FluentValidation;

namespace FitPro.BusinessLogic
{
    public class EditAlimentValidation : AbstractValidator<EditAlimentModel>
    {
        public EditAlimentValidation(NutritionistService service)
        {
            RuleFor(u => u.Fat)
                .Must(r => r >= 0 && r <= 1000).WithMessage("This aliment is already in our list");

            RuleFor(u => u.Protein)
                .Must(r => r >= 0 && r <= 1000).WithMessage("This aliment is already in our list");

            RuleFor(u => u.Carbo)
                .Must(r => r >= 0 && r <= 1000).WithMessage("This aliment is already in our list");

            RuleFor(u => u.Calories)
                .NotEmpty().WithMessage("The calories field is required.")
                .Must(r => r >= 0 && r <= 10000).WithMessage("This aliment is already in our list");

            RuleFor(u => u.NutritionistPassword)
                .NotEmpty().WithMessage("The nutritionist password field is required");

            When(r => r.NutritionistPassword != null, () =>
            {
                RuleFor(r => new { r.Nutritionist, r.NutritionistPassword })
               .Must(x => service.CheckMatchPassword(x.Nutritionist, x.NutritionistPassword)).WithMessage("Password is incorrect.");
            });

            
        }
    }
}
