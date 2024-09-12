using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class LoginViewModel
    {
        [Display(Name = "Email"), StringLength(50), Required(ErrorMessage = "{0} Boş Geçilemez!"), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name = "Şifre"), StringLength(18), Required(ErrorMessage = "Şifre Boş Geçilemez!"), DataType(DataType.Password)]
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
