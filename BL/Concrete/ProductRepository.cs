using BL.Abstract;
using DAL;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace BL.Concrete
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(DatabaseContext context) : base(context)
        {

        }
        public async Task<IEnumerable<Product>> GetAllCategoriesBySlugsAsync()
        {
            return await context.Products.Include(x => x.Category).ToListAsync();
        }

        public async Task<Product> GetWithSlugByIdAsync(int categoryId)
        {
            return await context.Products.Include(x => x.Category).FirstOrDefaultAsync();
        }
    }
}
