using Entities;

namespace BL.Abstract
{
    public interface IProductService : IRepository<Product>
    {
        Task<Product> GetWithCategoryByIdAsync(int categoryId);
        Task<IEnumerable<Product>> GetAllCategoriesByProductsAsync();
    }
}
