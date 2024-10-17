using Core.Entities;

namespace NetCoreUrunSitesi.Models
{
    public class CartViewModel
    {
        public List<CartLine>? CartProducts { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
