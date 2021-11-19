using FluentValidation;

namespace FitPro.BusinessLogic
{
    public class DeleteWorkoutValidation : AbstractValidator<DeleteWorkoutModel>
    {
        public DeleteWorkoutValidation(BaseService service)
        {
            RuleFor(r => r.TrainerPassword)
                .NotEmpty().WithMessage("The trainer password is required");

            When(r => r.TrainerPassword != null, () =>
            {
                RuleFor(r => new { r.IdTrainer, r.TrainerPassword })
               .Must(x => service.CheckMatchPassword(x.IdTrainer, x.TrainerPassword)).WithMessage("Password is incorrect.");
            });

            
        }
    }
}
