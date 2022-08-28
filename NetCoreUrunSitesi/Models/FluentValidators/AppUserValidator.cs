using Entities;
using FluentValidation;

namespace NetCoreUrunSitesi.Models.FluentValidators
{
    public class AppUserValidator : AbstractValidator<AppUser>
    {
        public AppUserValidator()
        {
            RuleFor(model => model.Name).NotNull().NotEmpty().WithMessage("Please specify a name");
            RuleFor(model => model.Email).NotNull().NotEmpty().Length(3, 10);
            RuleFor(model => model.Id).GreaterThanOrEqualTo(0);
            RuleFor(model => model.Password).NotEqual("").When(model => model.Id > 0)
                .WithMessage("Please specify a price");
        }
    }
}
