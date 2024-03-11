using Core.DTOs;
using Core.Entities;

namespace Service.Abstract
{
    public interface ICategoryService : IService<Category>
    {
        Task<Category> GetCategoryByProductsAsync(int id);
        Task<IList<CategorySelectDto>> GetCategoriesByIdNameAsync();
    }
}
