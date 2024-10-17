using Core.Entities;

namespace Service.Abstract
{
    public interface ICartService
    {
        void AddProduct(Product product, int quantity);
        void RemoveProduct(Product product);
        decimal TotalPrice();
        void ClearAll();
    }
}
