using Core.Entities;

namespace WebAPIUsing.Models
{
    public class ProductDetailViewModel
    {
        public Product Product { get; set; }
        public List<Product>? RelatedProducts { get; set; }
    }
}
