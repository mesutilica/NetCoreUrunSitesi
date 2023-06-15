namespace Core.Entities
{
    public class Cart
    {
        private List<CartLine> products = new List<CartLine>();
        public List<CartLine> Products => products;

        public void AddProduct(Product product, int quantity)
        {
            var prd = products
                .Where(i => i.Product.Id == product.Id)
                .FirstOrDefault();

            if (prd == null)
            {
                products.Add(new CartLine()
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
            products.RemoveAll(i => i.Product.Id == product.Id);
        }

        public decimal TotalPrice()
        {
            return products.Sum(i => i.Product.Price * i.Quantity);
        }

        public void ClearAll()
        {
            products.Clear();
        }

    }
}
