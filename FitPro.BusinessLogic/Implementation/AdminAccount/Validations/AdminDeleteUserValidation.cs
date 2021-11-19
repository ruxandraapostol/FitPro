using FluentValidation;

namespace FitPro.BusinessLogic
{
    public class AdminDeleteUserValidation : AbstractValidator<AdminDeleteUserModel>
    {
        public AdminDeleteUserValidation(AdminService service)
        {
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
