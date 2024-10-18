using Core.Entities;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Service.Abstract;

namespace Service.Concrete
{
    public class ProductService : Service<Product>, IProductService
    {
        IMemoryCache _memoryCache;
        const string key = "products";
        public ProductService(DatabaseContext context, IMemoryCache memoryCache) : base(context)
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
