using Core.Entities;
using DAL.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace DAL.Concrete
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        IMemoryCache _memoryCache;
        const string key = "products";
        public ProductRepository(DatabaseContext context, IMemoryCache memoryCache) : base(context)
        {
            _memoryCache = memoryCache;
            if (!_memoryCache.TryGetValue(key, out _))
            {
                _memoryCache.Set(key, _dbSet.ToListAsync().Result);
            }
        }
        public async Task<IEnumerable<Product>> GetAllProductsByCategoriesBrandsAsync()
        {
            return await _context.Products.Include(x => x.Category).Include(x => x.Brand).ToListAsync();
        }

        public Task<List<Product>> GetAllProductsByCacheAsync()
        {
            var list = _memoryCache.Get<List<Product>>(key);
            return Task.FromResult(list);
        }

        public async Task<Product> GetProductByCategoryAndBrandAsync(int id)
        {
            return await _context.Products.Include(c => c.Category).Include(b => b.Brand).FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
