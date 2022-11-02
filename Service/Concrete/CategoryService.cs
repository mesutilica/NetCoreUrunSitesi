using DAL;
using Entities;
using Microsoft.EntityFrameworkCore;
using Service.Abstract;

namespace Service.Concrete
{
    public class CategoryService : Service<Category>, ICategoryService
    {
        public CategoryService(DatabaseContext _context) : base(_context)
        {
        }

        public async Task<Category> GetCategoryWithProductsByCategoryIdAsync(int categoryId)
        {
            return await context.Categories.Include(p => p.Products).FirstOrDefaultAsync(c => c.Id == categoryId);
        }
    }
}
