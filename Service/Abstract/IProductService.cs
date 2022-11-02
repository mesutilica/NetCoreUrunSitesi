using Entities;

namespace Service.Abstract
{
    public interface IProductService : IService<Product>
    {
        Task<Product> GetProductByCategoryAndBrandAsync(int id);
        Task<IEnumerable<Product>> GetAllCategoriesByProductsAsync();
        Task<List<Product>> GetAllProductsByCacheAsync();
    }
}
