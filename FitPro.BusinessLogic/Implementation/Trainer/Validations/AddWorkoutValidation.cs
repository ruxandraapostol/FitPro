using FluentValidation;

namespace FitPro.BusinessLogic
{
    public class AddWorkoutValidation : AbstractValidator<AddWorkoutModel>
    {
        public AddWorkoutValidation(BaseService service)
        {
            RuleFor(u => u.Name)
                .NotEmpty().WithMessage("The Name field is required")
                .MaximumLength(50).WithMessage("The Name should not be longer than 50 charcters");

            RuleFor(u => u.Description)
                .NotEmpty().WithMessage("The Description field is required")
                .MaximumLength(500).WithMessage("The Description should not be longer than 500 charcters");

            RuleFor(u => u.Time)
                .NotEmpty().WithMessage("The Time of a Workout field is required")
                .Must(u => u > 0).WithMessage("A workout cannot take you back in the past. Please introduce a pozitive number for the workout required time.")
                .Must(u => u < 200).WithMessage("A workout should be enjoyable and effective, but moderate. For this reason we consider that the required time should not be longer than 200 minutes.");
                
            RuleFor(u => u.Calories)
                .NotEmpty().WithMessage("The Number of burnt calories field is required")
                .Must(u => u > 0).WithMessage("A workkout consume energy(calories). Please introduce a pozitive number for the workout burnt calories.")
                .Must(u => u < 1000).WithMessage("A workout should be effective and healthy, but moderate. For this reason we consider that the required number of burnt calories per workout should not be greater than 1000.");

            RuleFor(u => u.LinkUrl)
                .NotEmpty().WithMessage("The Workout Video UrlLink field is required")
                .Must(service.CheckLinkNotAlreadyUsed).WithMessage("This UrlLink has already been posted.");

            RuleFor(u => u.TrainerPassword)
                .NotEmpty().WithMessage("The Trainer Password field is required");


            When(r => r.TrainerPassword != null, () =>
            {
                RuleFor(r => new { r.IdTrainer, r.TrainerPassword })
               .Must(x => service.CheckMatchPassword(x.IdTrainer, x.TrainerPassword)).WithMessage("Password is incorrect.");
            });
        }
    }
}
