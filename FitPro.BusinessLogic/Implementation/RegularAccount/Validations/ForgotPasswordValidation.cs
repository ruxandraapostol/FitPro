using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitPro.BusinessLogic
{
    public class ForgotPasswordValidation : AbstractValidator<ForgotPasswordModel>
    {
        public ForgotPasswordValidation(RegularUserAccountService service)
        {
            RuleFor(u => u.Email)
                   .NotEmpty().WithMessage("The Email field is required")
                   .EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible).WithMessage("Please introduce a valid Email address")
                   .Must(email => !service.NotAlreadyExistEmail(email)).WithMessage("This Email has no associated account.");
        }
    }
}
