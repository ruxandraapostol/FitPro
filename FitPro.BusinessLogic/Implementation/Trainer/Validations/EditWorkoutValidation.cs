using FluentValidation;

namespace FitPro.BusinessLogic
{
    public class EditWorkoutValidation : AbstractValidator<EditWorkoutModel>
    {
        public EditWorkoutValidation(BaseService service)
        {
            RuleFor(u => u.Description)
                .NotEmpty().WithMessage("The Description field is required")
                .MaximumLength(500).WithMessage("The Description should not be longer than 500 charcters");

            RuleFor(u => u.Time)
                .NotEmpty().WithMessage("The Time of a Workout field is required")
                .Must(u => u < 200).WithMessage("A workout should be enjoyable and effective, but moderate. For this reason we consider that the required time should not be longer than 200 minutes.");

            RuleFor(u => u.Calories)
                .NotEmpty().WithMessage("The Number of burnt calories field is required")
                .Must(u => u < 1000).WithMessage("A workout should be effective and healthy, but moderate. For this reason we consider that the required number of burnt calories per workout should not be greater than 1000.");

            RuleFor(u => u.TrainerPassword)
                .NotEmpty().WithMessage("The Trainer Password field is required");

            When(r => r.TrainerPassword != null && r.LastModifiedBy != System.Guid.Empty, () =>
            {
                RuleFor(r => new { r.LastModifiedBy, r.TrainerPassword })
               .Must(x => service.CheckMatchPassword(x.LastModifiedBy, x.TrainerPassword)).WithMessage("Password is incorrect.");
            });


        }
    }
}
