using Core.Entities;

namespace DAL.Abstract
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category> GetCategoryByProductsAsync(int id);
    }
}
