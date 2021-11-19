using FluentValidation;

namespace FitPro.BusinessLogic
{
    public class AdminAddUserValidation : AbstractValidator<AdminAddUserModel>
    {
        public AdminAddUserValidation(AdminService service)
        {
            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("The New User Email field is required")
                .EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible).WithMessage("Please introduce a valid Email address")
                .Must(service.NotAlreadyExistEmail).WithMessage("This Email is already used.");

            RuleFor(r => r.RegisterPassword)
                .NotEmpty().WithMessage("The New User Password field is required")
                .Must(r => r.Length > 9).WithMessage("The New User Password field should at least contains 10 characters")
                .Must(service.PasswordHasUpperLetter).WithMessage("The New User Password must contains an upper letter")
                .Must(service.PasswordHasLowerLetter).WithMessage("The New User Password must contains a lower letter")
                .Must(service.PasswordHasNumber).WithMessage("The New User Password must contains a number")
                .Must(service.PasswordHasSpecialLetter).WithMessage("The New User Password must contains a special character");

            RuleFor(r => r.RegisterConfirmPassword)
                .NotEmpty().WithMessage("The New User Confirme Password field is required");

            RuleFor(r => r.FirstName)
                .NotEmpty().WithMessage("The New User First Name field is required");

            RuleFor(r => r.LastName)
                .NotEmpty().WithMessage("The New User Last Name field is required");

            RuleFor(r => r.UserName)
                .NotEmpty().WithMessage("The New User Username field is required")
                .Must(service.NotAlreadyExistUsername).WithMessage("This Usename is already used.");

            RuleFor(r => r.RegisterConfirmPassword)
               .NotEmpty().WithMessage("The Admin Password field is required");

            RuleFor(r => r.AdminPassword)
                .NotEmpty().WithMessage("The admin password is required");


            When(r => r.AdminPassword != null, () =>
            {
                RuleFor(r => new { r.IdAdmin, r.AdminPassword })
               .Must(x => service.CheckMatchPassword(x.IdAdmin, x.AdminPassword)).WithMessage("Password is incorrect.");
            });
        }
    }
}
