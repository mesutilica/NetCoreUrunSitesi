﻿using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Brand : IEntity
    {
        public int Id { get; set; }
        [Display(Name = "Ad"), StringLength(50)]
        public string? Name { get; set; }
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }
        [StringLength(50)]
        public string? Logo { get; set; }
        [Display(Name = "Aktif?")]
        public bool IsActive { get; set; }
        [Display(Name = "Eklenme Tarihi"), ScaffoldColumn(false)] // ScaffoldColumn(false) crud sayfaları oluşturulurken bu kolonun ekranda oluşmamasını sağlar
        public DateTime CreateDate { get; set; }
        public ICollection<Product> Products { get; set; }
        public Brand()
        {
            Products = new List<Product>();
        }
    }
}
