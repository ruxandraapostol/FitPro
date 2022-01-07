using FluentValidation;

namespace FitPro.BusinessLogic
{
    public class SaveAlimentTrackValidation : AbstractValidator<SaveAlimentTrackModel>
    {
        public SaveAlimentTrackValidation()
        {
            RuleFor(u => u.Quantity)
                .NotEmpty().WithMessage("This field is mandatory for a good nutrition tracking.")
                .Must(r => r >= 0).WithMessage("The quantity cannot be less than 0 grams.")
                .Must(r => r <= 10000).WithMessage("The quantity cannot be less than 10000 grams.");

            RuleFor(u => u.IdAliment)
                .NotEmpty().WithMessage("This field is mandatory for a good nutrition tracking.");

            RuleFor(u => u.Date)
                .NotEmpty().WithMessage("Internal server error.");
            
            RuleFor(u => u.IdRegularUser)
                .NotEmpty().WithMessage("Internal server error.");
        }
    }
}
