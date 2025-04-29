using Core.Entities;
using Service.Abstract;

namespace Service.Concrete
{
    public class CartService : ICartService
    {
        public List<CartLine> CartLines = new();

        public bool AddProduct(Product product, int quantity)
        {
            var urun = CartLines.FirstOrDefault(p => p.Product.Id == product.Id);

            if (urun == null)
            {
                CartLines.Add(new CartLine()
                {
                    Product = product,
                    Quantity = quantity
                });
                return false;
            }
            else
            {
                urun.Quantity += quantity;
                return true;
            }
        }

        public void UpdateProduct(Product product, int quantity)
        {
            var urun = CartLines.FirstOrDefault(p => p.Product.Id == product.Id);

            if (urun == null)
            {
                CartLines.Add(new CartLine()
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                urun.Quantity = quantity;
            }
        }

        public void RemoveProduct(Product product)
        {
            CartLines.RemoveAll(p => p.Product.Id == product.Id);
        }

        public decimal TotalPrice()
        {
            return CartLines.Sum(c => c.Product.Price * c.Quantity);
        }

        public void ClearAll()
        {
            CartLines.Clear();
        }
    }
}
