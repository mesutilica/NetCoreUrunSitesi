using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Favorite : IEntity
    {
        public int Id { get; set; }
        [Display(Name = "Ürün")]
        public int? ProductId { get; set; }
        [Display(Name = "Ürün")]
        public Product? Product { get; set; }
        [Display(Name = "Kullanıcı")]
        public int? AppUserId { get; set; }
        [Display(Name = "Kullanıcı")]
        public AppUser? AppUser { get; set; }
        [Display(Name = "Eklenme Tarihi"), ScaffoldColumn(false)] 
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
