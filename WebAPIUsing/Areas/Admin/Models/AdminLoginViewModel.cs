using System.ComponentModel.DataAnnotations;

namespace WebAPIUsing.Models
{
    public class AdminLoginViewModel
    {
        [Display(Name = "Kullanıcı Adı"), StringLength(50), Required(ErrorMessage = "Kullanıcı Adı Boş Geçilemez!")]
        public string UserName { get; set; }
        [Display(Name = "Şifre"), StringLength(50), Required(ErrorMessage = "Şifre Boş Geçilemez!")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
