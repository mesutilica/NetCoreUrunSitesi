using Core.Entities;

namespace NetCoreUrunSitesi.Models
{
    public class CheckoutViewModel
    {
        public List<CartLine>? CartProducts { get; set; }
        public decimal TotalPrice { get; set; }
        public AppUser? AppUser { get; set; }
    }
}
