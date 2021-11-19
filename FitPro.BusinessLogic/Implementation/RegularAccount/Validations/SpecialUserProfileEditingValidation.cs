using FluentValidation;
using System;
using System.Linq;
using BCryptNet = BCrypt.Net.BCrypt;

namespace FitPro.BusinessLogic
{
    public class SpecialUserProfileEditingValidation : AbstractValidator<SpecialProfileModel>
    {

        public SpecialUserProfileEditingValidation(RegularUserAccountService service)
        {
            RuleFor(r => r.Password)
               .NotEmpty().WithMessage("The Password field is required");

            RuleFor(r => r.FirstName)
                .NotEmpty().WithMessage("The First Name field is required");

            RuleFor(r => r.LastName)
                .NotEmpty().WithMessage("The Last Name field is required");

            RuleFor(r => r.UserName)
                .NotEmpty().WithMessage("The Username field is required");

            When(r => r.UserName != null && r.Email != null, () =>
            {
                RuleFor(r => new { r.UserName, r.Email })
                    .Must(x => service.NotAlreadyExistUsername(x.UserName, x.Email)).WithMessage("This username is already used.");
            });

            When(r => r.Password != null, () =>
            {
                RuleFor(r => new { r.Email, r.Password })
                .Must(x => service.CheckMatchPasswordEmail(x.Email, x.Password)).WithMessage("Incorrect password");
            });
        }

    }
}