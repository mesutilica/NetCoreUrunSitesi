using BL.Abstract;
using DAL;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace BL.Concrete
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        //private DatabaseContext _appDbContext { get => _context as DatabaseContext; }
        public ProductRepository(DatabaseContext context) : base(context)
        {

        }
        public async Task<IEnumerable<Product>> GetAllCategoriesByProductsAsync()
        {
            return await context.Products.Include(x => x.Category).ToListAsync();
        }

        public async Task<Product> GetWithCategoryByIdAsync(int categoryId)
        {
            return await context.Products.Include(x => x.Category).FirstOrDefaultAsync();
        }
    }
}
