using BL.Abstract;
using Entities;

namespace BL.Concrete
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public Task<IEnumerable<Category>> GetAllCategoriesBySlugsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Category> GetWithSlugByIdAsync(int categoryId)
        {
            throw new NotImplementedException();
        }
    }
}
