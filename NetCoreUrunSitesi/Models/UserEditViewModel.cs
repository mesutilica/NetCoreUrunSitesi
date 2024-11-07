using System.ComponentModel.DataAnnotations;

namespace NetCoreUrunSitesi.Models
{
    public class UserEditViewModel
    {
        [Display(Name = "Adı")]
        public string Name { get; set; }
        [Display(Name = "Soyadı")]
        public string Surname { get; set; }
        public string Email { get; set; }
        [Display(Name = "Telefon")]
        public string? Phone { get; set; }
        [Display(Name = "Şifre")]
        public string Password { get; set; }
    }
}
