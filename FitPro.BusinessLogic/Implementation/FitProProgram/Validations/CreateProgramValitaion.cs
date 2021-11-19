using FluentValidation;
using System.Linq;

namespace FitPro.BusinessLogic
{
    public class CreateProgramValitaion : AbstractValidator<CreateProgramModel>
    {
        public CreateProgramValitaion()
        {
            RuleFor(u => u.TimePeriod)
                .NotEmpty().WithMessage("The time for this program is requiered");

            RuleFor(u => u.SelectedCategories)
                .Must(u => u.Count() > 2).WithMessage("Please select at least 3 categories that you want or enjoy to workout.");
        }
    }
}
