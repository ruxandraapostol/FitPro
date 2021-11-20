using FluentValidation;

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
                .Must(r => r >= 0).WithMessage("The quantity cannot be less than 0 grams.")
                .Must(r => r <= 10000).WithMessage("The quantity cannot be greater than 1000 per 100 grams.");

            RuleFor(u => u.Protein)
                .Must(r => r >= 0).WithMessage("The quantity cannot be less than 0 grams.")
                .Must(r => r <= 10000).WithMessage("The quantity cannot be greater than 1000 per 100 grams.");

            RuleFor(u => u.Carbo)
                .Must(r => r >= 0).WithMessage("The carbo cannot be less than 0 per 100 grams.")
                .Must(r => r <= 10000).WithMessage("The carbo cannot be greater than 1000 per 100 grams.");

            RuleFor(u => u.Calories)
                .NotEmpty().WithMessage("The calories field is required.")
                .Must(r => r >= 0).WithMessage("The calories cannot be less than 0 per 100 grams.")
                .Must(r => r <= 10000).WithMessage("The calories cannot be greater than 10000 per 100 grams.");

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
