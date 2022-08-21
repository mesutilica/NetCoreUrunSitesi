using Entities;

namespace BL.Abstract
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetWithCategoryByIdAsync(int categoryId);
        Task<IEnumerable<Product>> GetAllCategoriesByProductsAsync();
    }
}
