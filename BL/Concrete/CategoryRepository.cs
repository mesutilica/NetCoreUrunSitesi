using BL.Abstract;
using DAL;
using Entities;

namespace BL.Concrete
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(DatabaseContext context) : base(context)
        {

        }
        public Task<IEnumerable<Category>> GetAllCategoriesBySlugsAsync()
        {
            return null;
        }

        public Task<Category> GetWithSlugByIdAsync(int categoryId)
        {
            throw new NotImplementedException();
        }
    }
}
