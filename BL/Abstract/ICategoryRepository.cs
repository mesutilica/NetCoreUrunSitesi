using Entities;

namespace BL.Abstract
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category> GetWithSlugByIdAsync(int categoryId);
        Task<IEnumerable<Category>> GetAllCategoriesBySlugsAsync();
    }
}
