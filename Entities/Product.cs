using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Product : IEntity
    {
        public int Id { get; set; }
        [Display(Name = "Ad"), StringLength(50)]
        public string Name { get; set; }
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }
        [Display(Name = "Resim"), StringLength(50)]
        public string? Image { get; set; }
        [Display(Name = "Fiyat")]
        public decimal Price { get; set; }
        [Display(Name = "Stok")]
        public int Stock { get; set; }
        [Display(Name = "Aktif?")]
        public bool IsActive { get; set; }
        [Display(Name = "Eklenme Tarihi"), ScaffoldColumn(false)] // ScaffoldColumn(false) crud sayfaları oluşturulurken bu kolonun ekranda oluşmamasını sağlar
        public DateTime CreateDate { get; set; }
        [Display(Name = "Ürün Kategorisi")]
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        [Display(Name = "Ürün Markası")]
        public int? BrandId { get; set; }
        public virtual Brand? Brand { get; set; }
    }
}
