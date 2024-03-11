using Core.Entities;

namespace Service.Abstract
{
    public interface IProductService : IService<Product>
    {
        Task<Product> GetProductByCategoryAndBrandAsync(int id);
        Task<IEnumerable<Product>> GetAllProductsByCategoriesBrandsAsync();
        Task<List<Product>> GetAllProductsByCacheAsync();
    }
}
