using Core.Entities;
using Service.Abstract;

namespace Service.Concrete
{
    public class CartService : ICartService
    {
        private List<CartLine> CartLines = new List<CartLine>();
        public List<CartLine> Products => CartLines;

        public void AddProduct(Product product, int quantity)
        {
            var prd = CartLines
                .Where(i => i.Product.Id == product.Id)
                .FirstOrDefault();

            if (prd == null)
            {
                CartLines.Add(new CartLine()
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                prd.Quantity += quantity;
            }
        }

        public void RemoveProduct(Product product)
        {
            CartLines.RemoveAll(i => i.Product.Id == product.Id);
        }

        public decimal TotalPrice()
        {
            return CartLines.Sum(i => i.Product.Price * i.Quantity);
        }

        public void ClearAll()
        {
            CartLines.Clear();
        }
    }
}
