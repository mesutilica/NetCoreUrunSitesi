using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Address : IEntity
    {
        public int Id { get; set; }
        [Display(Name = "Adres Başlığı"), StringLength(50), Required(ErrorMessage = "{0} Alanı Zorunludur!")]
        public string Title { get; set; }
        [Display(Name = "Şehir"), StringLength(50), Required(ErrorMessage = "{0} Alanı Zorunludur!")]
        public string City { get; set; }
        [Display(Name = "İlçe"), StringLength(50), Required(ErrorMessage = "{0} Alanı Zorunludur!")]
        public string District { get; set; }
        [Display(Name = "Açık Adres"), DataType(DataType.MultilineText), Required(ErrorMessage = "{0} Alanı Zorunludur!")]
        public string OpenAddress { get; set; }
        [Display(Name = "Aktif")]
        public bool IsActive { get; set; } = true;
        [Display(Name = "Fatura Adresi")]
        public bool IsBillingAddress { get; set; } = true;
        [Display(Name = "Teslimat Adresi")]
        public bool IsDeliveryAddress { get; set; } = true;
        [Display(Name = "Eklenme Tarihi"), ScaffoldColumn(false)]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [ScaffoldColumn(false)]
        public Guid AddressGuid { get; set; } = Guid.NewGuid();
        public int? AppUserId { get; set; }
        public virtual AppUser? AppUser { get; set; }
    }
}
