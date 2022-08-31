using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class AppUserLogin
    {
        [Required(ErrorMessage = "Kullanıcı adı gereklidir")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Parola gereklidir")]
        public string Password { get; set; }
    }
}
