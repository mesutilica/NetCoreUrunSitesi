using Entities;

namespace Service.Abstract
{
    public interface ICategoryService : IService<Category>
    {
        Task<Category> GetCategoryWithProductsByCategoryIdAsync(int categoryId);
    }
}
