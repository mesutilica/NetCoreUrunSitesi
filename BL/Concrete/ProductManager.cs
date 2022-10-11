using BL.Abstract;
using DAL;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace BL.Concrete
{
    public class ProductManager : Repository<Product>, IProductService
    {
        //private DatabaseContext _appDbContext { get => _context as DatabaseContext; }
        public ProductManager(DatabaseContext context) : base(context)
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
