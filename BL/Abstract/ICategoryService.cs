using Entities;

namespace BL.Abstract
{
    public interface ICategoryService : IRepository<Category>
    {
        Task<Category> GetCategoryWithProductsByCategoryIdAsync(int categoryId);
    }
}
