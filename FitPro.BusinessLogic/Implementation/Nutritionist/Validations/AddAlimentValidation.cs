using FluentValidation;
using System.Linq;

namespace FitPro.BusinessLogic
{
    public class AddAlimentValidation : AbstractValidator<AddAlimentModel>
    {
        public AddAlimentValidation(NutritionistService service)
        {
            RuleFor(u => u.Name)
                .NotEmpty().WithMessage("The name for aliment is required.")
                .MaximumLength(100).WithMessage("The name of an aliment should not be longer than 200 characters.")
                .Must(service.AlimentNotAlreadyExist).WithMessage("This aliment is already in our list");

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
                RuleFor(r => new { r.IdNutritionist, r.NutritionistPassword })
               .Must(x => service.CheckMatchPassword(x.IdNutritionist, x.NutritionistPassword)).WithMessage("Password is incorrect.");
            });

        }
    }
}
