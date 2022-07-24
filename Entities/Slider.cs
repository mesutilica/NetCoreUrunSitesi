using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Slider : IEntity
    {
        public int Id { get; set; }
        [Display(Name = "Başlık"), StringLength(50)]
        public string? Name { get; set; }
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }
        [Display(Name = "Resim"), StringLength(100)]
        public string? Image { get; set; }
        [Display(Name = "Resim Link"), StringLength(100)]
        public string? Link { get; set; }
    }
}
