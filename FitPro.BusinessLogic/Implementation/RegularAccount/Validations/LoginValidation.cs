using FluentValidation;
using System.Linq;
using BCryptNet = BCrypt.Net.BCrypt;

namespace FitPro.BusinessLogic
{
    public class LoginValidation : AbstractValidator<LoginModel>
    {
        public LoginValidation(RegularUserAccountService service)
        {
            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("The Email field is required");

            RuleFor(r => r.Password)
                .NotEmpty().WithMessage("The Password field is required");

            When(r => r.Password != null && r.Email != null, () =>
            {
                RuleFor(r => new { r.Email, r.Password })
                .Must(x => service.CheckMatchPasswordEmail(x.Email, x.Password)).WithMessage("Password or email are incorrect.");
            });

        }
    }
}
