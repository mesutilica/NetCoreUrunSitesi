using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class LoginViewModel
    {
        [Display(Name = "Email"), StringLength(50), Required(ErrorMessage = "{0} Boş Geçilemez!")]
        public string Email { get; set; }
        [Display(Name = "Şifre"), StringLength(50), Required(ErrorMessage = "Şifre Boş Geçilemez!")]
        public string Password { get; set; }
    }
}
