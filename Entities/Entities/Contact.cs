using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Contact : IEntity
    {
        public int Id { get; set; }
        [Display(Name = "Ad"), StringLength(50), Required(ErrorMessage = "{0} Alanı Gereklidir!")]
        public string Name { get; set; }
        [Display(Name = "Soyad"), StringLength(50), Required(ErrorMessage = "{0} Alanı Gereklidir!")]
        public string Surname { get; set; }
        [StringLength(50)]
        public string? Email { get; set; }
        [Display(Name = "Telefon"), StringLength(15)]
        public string? Phone { get; set; }
        [Display(Name = "Mesaj"), DataType(DataType.MultilineText), Required(ErrorMessage = "{0} Alanı Gereklidir!")]
        public string Message { get; set; }
        [Display(Name = "Mesaj Tarihi"), ScaffoldColumn(false)]
        public DateTime? CreateDate { get; set; } = DateTime.Now; // Boş geçilebilsin, boş geçilirse o anki zaman otomatik eklensin
    }
}
