using BL.Abstract;
using DAL;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BL.Concrete
{
    public class ProductManager : Repository<Product>, IProductService
    {
        IMemoryCache _memoryCache;
        const string key = "products";
        //private DatabaseContext _appDbContext { get => _context as DatabaseContext; }
        public ProductManager(DatabaseContext context, IMemoryCache memoryCache) : base(context)
        {
            _memoryCache = memoryCache;
            if (!_memoryCache.TryGetValue(key, out _))
            {
                _memoryCache.Set(key, dbSet.ToListAsync().Result);
            }
        }
        public async Task<IEnumerable<Product>> GetAllCategoriesByProductsAsync()
        {
            return await context.Products.Include(x => x.Category).ToListAsync();
        }

        public Task<List<Product>> GetAllProductsByCacheAsync()
        {
            var list = _memoryCache.Get<List<Product>>(key);
            return Task.FromResult(list);
        }

        public async Task<Product> GetWithCategoryByIdAsync(int categoryId)
        {
            return await context.Products.Include(x => x.Category).FirstOrDefaultAsync();
        }
    }
}
