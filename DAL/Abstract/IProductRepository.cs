using Core.Entities;

namespace DAL.Abstract
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetProductByCategoryAndBrandAsync(int id);
        Task<IEnumerable<Product>> GetAllProductsByCategoriesBrandsAsync();
        Task<List<Product>> GetAllProductsByCacheAsync();
    }
}
