using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class AppUser : IEntity
    {
        public int Id { get; set; }
        [Display(Name = "Ad"), StringLength(50)]
        public string? Name { get; set; }
        [Display(Name = "Soyad"), StringLength(50)]
        public string? Surname { get; set; }
        [StringLength(50)]
        public string? Email { get; set; }
        [Display(Name = "Telefon"), StringLength(15)]
        public string? Phone { get; set; }
        [Display(Name = "Kullanıcı Adı"), StringLength(50), Required(ErrorMessage = "Boş Geçilemez!")]
        public string Username { get; set; }
        [Display(Name = "Şifre"), StringLength(50), Required(ErrorMessage = "Boş Geçilemez!")]
        public string Password { get; set; }
        [Display(Name = "Aktif?")]
        public bool IsActive { get; set; }
        [Display(Name = "Admin?")]
        public bool IsAdmin { get; set; }
        [Display(Name = "Eklenme Tarihi"), ScaffoldColumn(false)]
        public DateTime CreateDate { get; set; }
    }
}
