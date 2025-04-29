using Core.Entities;

namespace Service.Abstract
{
    public interface ICartService
    {
        bool AddProduct(Product product, int quantity);
        void UpdateProduct(Product product, int quantity);
        void RemoveProduct(Product product);
        decimal TotalPrice();
        void ClearAll();
    }
}
