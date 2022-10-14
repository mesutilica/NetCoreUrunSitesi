using BL.Abstract;
using DAL;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace BL.Concrete
{
    public class CategoryManager : Repository<Category>, ICategoryService
    {
        public CategoryManager(DatabaseContext context) : base(context)
        {

        }

        public async Task<Category> GetCategoryWithProductsByCategoryIdAsync(int categoryId)
        {
            //return await context.Categories.Include(p => p.Products).FirstOrDefaultAsync(c => c.Id == categoryId);
            return await dbSet.Include(p => p.Products).FirstOrDefaultAsync(c => c.Id == categoryId);
        }
    }
}
