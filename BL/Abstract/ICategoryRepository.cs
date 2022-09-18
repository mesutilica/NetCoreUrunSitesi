using Entities;

namespace BL.Abstract
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category> GetCategoryWithProductsByCategoryIdAsync(int categoryId);
        Task<IEnumerable<Category>> GetAllCategoriesBySlugsAsync();
    }
}
