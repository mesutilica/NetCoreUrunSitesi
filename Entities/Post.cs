using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Post : IEntity
    {
        public int Id { get; set; }
        [Display(Name = "Ad"), StringLength(50)]
        public string Name { get; set; }
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }
        [Display(Name = "Resim"), StringLength(50)]
        public string? Image { get; set; }
        [Display(Name = "Aktif?")]
        public bool IsActive { get; set; }
        [Display(Name = "Eklenme Tarihi"), ScaffoldColumn(false)] // ScaffoldColumn(false) crud sayfaları oluşturulurken bu kolonun ekranda oluşmamasını sağlar
        public DateTime CreateDate { get; set; }
        [Display(Name = "Sayfa Kategorisi")]
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
    }
}
