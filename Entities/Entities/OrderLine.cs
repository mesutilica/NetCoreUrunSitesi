using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class OrderLine : IEntity
    {
        public int Id { get; set; }
        [Display(Name = "Sipariş No")]
        public int OrderId { get; set; }
        [Display(Name = "Sipariş No")]
        public virtual Order? Order { get; set; }
        [Display(Name = "Ürün No")]
        public int ProductId { get; set; }
        [Display(Name = "Ürün")]
        public virtual Product? Product { get; set; }
        [Display(Name = "Miktar")]
        public int Quantity { get; set; }
        [Display(Name = "Birim Fiyatı")]
        public decimal UnitPrice { get; set; }
    }
}
