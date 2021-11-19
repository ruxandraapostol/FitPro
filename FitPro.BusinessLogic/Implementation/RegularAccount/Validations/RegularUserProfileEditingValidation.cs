using FluentValidation;
using System;
using System.Linq;
using BCryptNet = BCrypt.Net.BCrypt;

namespace FitPro.BusinessLogic
{
    public class RegularUserProfileEditingValidation : AbstractValidator<RegularProfileModel>
    {
        public RegularUserProfileEditingValidation(RegularUserAccountService service)
        {
            RuleFor(r => r.Password)
               .NotEmpty().WithMessage("The Password field is required");

            RuleFor(r => r.FirstName)
                .NotEmpty().WithMessage("The First Name field is required");

            RuleFor(r => r.LastName)
                .NotEmpty().WithMessage("The Last Name field is required");


            var smallestDate = new DateTime(1900, 1, 1);
            RuleFor(r => r.BirthDay)
                .Must(r => !(r != null && DateTime.Compare(r.GetValueOrDefault(), DateTime.Today) > 0)).WithMessage("Birth date cannot be greater than today.")
                .Must(r => !(r != null && DateTime.Compare(r.GetValueOrDefault(), smallestDate) < 0)).WithMessage("Birth date cannot be smaller than 1900/01/01.");

            RuleFor(r => r.Height)
                .Must(r => !(r != null && r < 20)).WithMessage("Height cannot be smaller than 20cm")
                .Must(r => !(r != null && r > 300)).WithMessage("Height cannot be greater than 300cm");


            RuleFor(r => r.Weight)
                .Must(r => !(r != null && r < 0.2)).WithMessage("Weight cannot be smaller than 1kg")
                .Must(r => !(r != null && r > 650)).WithMessage("Height cannot be greater than 650kg");

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
