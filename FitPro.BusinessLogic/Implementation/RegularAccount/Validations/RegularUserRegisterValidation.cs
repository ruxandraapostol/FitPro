using FluentValidation;
using System.Linq;
using System;

namespace FitPro.BusinessLogic
{
    public class RegularUserRegisterValidation : AbstractValidator<RegularRegisterModel>
    {
        public RegularUserRegisterValidation(RegularUserAccountService service)
        {

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("The Email field is required")
                .EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible).WithMessage("Please introduce a valid Email address")
                .Must(service.NotAlreadyExistEmail).WithMessage("This Email is already used.");

            RuleFor(r => r.Password)
                .NotEmpty().WithMessage("The Password field is required")
                .Must(r => r.Length > 9).WithMessage("The Password field should at least contains 10 characters")
                .Must(service.PasswordHasUpperLetter).WithMessage("The Password must contains an upper letter")
                .Must(service.PasswordHasLowerLetter).WithMessage("The Password must contains a lower letter")
                .Must(service.PasswordHasNumber).WithMessage("The Password must contains a number")
                .Must(service.PasswordHasSpecialLetter).WithMessage("The Password must contains a special character");

            RuleFor(r => r.ConfirmPassword)
                .NotEmpty().WithMessage("The Password field is required");

            RuleFor(r => r.FirstName)
                .NotEmpty().WithMessage("The First Name field is required");

            RuleFor(r => r.LastName)
                .NotEmpty().WithMessage("The Last Name field is required");

            RuleFor(r => r.UserName)
                .NotEmpty().WithMessage("The Username field is required")
                .Must(service.NotAlreadyExistUsername).WithMessage("This Usename is already used.");

            var smallestDate = new DateTime(1900, 1, 1);
            RuleFor(r => r.BirthDay)
                .Must(r => !(r != null &&  DateTime.Compare(r.GetValueOrDefault(), DateTime.Today) > 0)).WithMessage("Birth date cannot be greater than today.")
                .Must(r => !(r != null && DateTime.Compare(r.GetValueOrDefault(), smallestDate) < 0)).WithMessage("Birth date cannot be smaller than 1900/01/01.");

            RuleFor(r => r.Height)
                .Must(r => !(r != null && r < 20)).WithMessage("Height cannot be smaller than 20cm")
                .Must(r => !(r != null && r > 300)).WithMessage("Height cannot be greater than 300cm");

            RuleFor(r => r.Weight)
                .Must(r => !(r != null && r < 0.2)).WithMessage("Weight cannot be smaller than 1kg")
                .Must(r => !(r != null && r > 650)).WithMessage("Height cannot be greater than 650kg");

        }
    }
}
