using Entities;

namespace DAL.Abstract
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetProductByCategoryAndBrandAsync(int id);
        Task<IEnumerable<Product>> GetAllCategoriesByProductsAsync();
        Task<List<Product>> GetAllProductsByCacheAsync();
    }
}
