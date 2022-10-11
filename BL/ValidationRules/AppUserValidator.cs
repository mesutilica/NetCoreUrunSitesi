using Entities;
using FluentValidation;

namespace BL.ValidationRules
{
    public class AppUserValidator : AbstractValidator<AppUser>
    {
        public AppUserValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Kullanıcı Adı Boş Geçilemez!");
            RuleFor(x => x.Surname).NotEmpty().WithMessage("Kullanıcı Soyadı Boş Geçilemez!");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email Boş Geçilemez!");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Şifre Boş Geçilemez!").MinimumLength(3).WithMessage("Şifre Minimum 3 Karakter Olmalı!");
            // Validationları tamamladıktan sonra UI a da nuget dan FluentValidation ekleyip Admin deki gibi kullanabiliyoruz
        }
    }
}
