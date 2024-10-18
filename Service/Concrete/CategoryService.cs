using Core.DTOs;
using Core.Entities;
using Data;
using Microsoft.EntityFrameworkCore;
using Service.Abstract;

namespace Service.Concrete
{
    public class CategoryService : Service<Category>, ICategoryService
    {
        public CategoryService(DatabaseContext context) : base(context)
        {
        }
        public async Task<Category> GetCategoryByProductsAsync(int id)
        {
            return await _dbSet.Where(c => c.Id == id).AsNoTracking().Include(c => c.Products).FirstOrDefaultAsync();
        }
        public async Task<IList<CategorySelectDto>> GetCategoriesByIdNameAsync()
        {
            return await _dbSet.Select(c => new CategorySelectDto
            {
                Id = c.Id,
                Name = c.Name
            }).AsNoTracking().ToListAsync();
        }
    }
}
