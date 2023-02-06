using Entities;
using FluentValidation;

namespace WebAPIUsing.Models.FluentValidators
{
    public class AppUserValidator : AbstractValidator<AppUser>
    {
        public AppUserValidator()
        {
            RuleFor(model => model.Name).NotNull().NotEmpty().WithMessage("Ad boş geçilemez!");
            RuleFor(model => model.Email).NotNull().NotEmpty();
            RuleFor(model => model.Id).GreaterThanOrEqualTo(0);
            RuleFor(model => model.Password).Length(3, 10).NotEqual("").When(model => model.Id > 0)
                .WithMessage("Şifre 0 dan büyük olmalı");
        }
    }
}
