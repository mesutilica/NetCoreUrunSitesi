using Core.Entities;
using FluentValidation;

namespace Service.ValidationRules
{
    public class AppUserValidator : AbstractValidator<AppUser>
    {
        public AppUserValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Surname).NotNull();
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email Boş Geçilemez!");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Şifre Boş Geçilemez!").MinimumLength(3).WithMessage("Şifre Minimum 3 Karakter Olmalı!");
            // Validationları tamamladıktan sonra UI a da nuget dan FluentValidation ekleyip Admin deki gibi kullanabiliyoruz
        }
    }
}
