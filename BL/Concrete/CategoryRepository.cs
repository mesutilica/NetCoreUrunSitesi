﻿using BL.Abstract;
using DAL;
using Entities;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Category> GetCategoryWithProductsByCategoryIdAsync(int categoryId)
        {
            return await context.Categories.Include(p => p.Products).FirstOrDefaultAsync(c => c.Id == categoryId);
        }
    }
}
