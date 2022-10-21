using Entities;

namespace Service.Abstract
{
    public interface IProductService : IService<Product>
    {
        Task<Product> GetWithCategoryByIdAsync(int categoryId);
        Task<IEnumerable<Product>> GetAllCategoriesByProductsAsync();
        Task<List<Product>> GetAllProductsByCacheAsync();
    }
}
