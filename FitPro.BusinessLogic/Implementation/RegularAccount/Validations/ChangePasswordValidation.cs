using FluentValidation;
using System.Linq;
using BCryptNet = BCrypt.Net.BCrypt;

namespace FitPro.BusinessLogic
{
    public class ChangePasswordValidation : AbstractValidator<ChangePasswordModel>
    {
        public ChangePasswordValidation(RegularUserAccountService service)
        {
            RuleFor(r => r.OldPassword)
               .NotEmpty().WithMessage("The Old Password field is required");

            RuleFor(r => r.NewPassword)
               .NotEmpty().WithMessage("The New Password field is required")
                .Must(r => r.Length > 9).WithMessage("The Password field should at least contains 10 characters")
                .Must(service.PasswordHasUpperLetter).WithMessage("The Password must contains an upper letter")
                .Must(service.PasswordHasLowerLetter).WithMessage("The Password must contains a lower letter")
                .Must(service.PasswordHasNumber).WithMessage("The Password must contains a number")
                .Must(service.PasswordHasSpecialLetter).WithMessage("The Password must contains a special character");

            RuleFor(r => r.ConfirmPassword)
               .NotEmpty().WithMessage("The Password field is required");

            RuleFor(r => r.Email)
                .NotEmpty().WithMessage("The Email field is required");

            When(r => r.OldPassword != null && r.Email != null, () =>
            {
                RuleFor(r => new { r.Email, r.OldPassword })
                .Must(x => service.CheckMatchPasswordEmail(x.Email, x.OldPassword)).WithMessage("Incorrect password");
            });

            When(r => r.OldPassword != null && r.Email != null, () =>
            {
                RuleFor(r => new { r.NewPassword, r.OldPassword })
                .Must(x => x.NewPassword != x.OldPassword).WithMessage("New Password should be different from Old Password.");
            });
        }
    }
}
