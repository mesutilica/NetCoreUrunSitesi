using BL.Abstract;
using Caching.Abstract;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Expressions;

namespace Caching.Concrete
{
    public class CacheService<T> : ICacheService<T> where T : class
    {
        private const string CacheKey = "dataCache";
        private readonly IMemoryCache _memoryCache;
        private readonly IRepository<T> _repository;

        public CacheService(IMemoryCache memoryCache, IRepository<T> repository)
        {
            _memoryCache = memoryCache;
            _repository = repository;
            if (!_memoryCache.TryGetValue(CacheKey, out _))
            {
                _memoryCache.Set(CacheKey, _repository.GetAllAsync().Result);
            }
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            var list = _memoryCache.Get<IEnumerable<T>>(CacheKey);
            return Task.FromResult(list);
        }

        public IQueryable<T> GetAllQueryable(Expression<Func<T, bool>> expression)
        {
            return _memoryCache.Get<List<T>>(CacheKey).Where(expression.Compile()).AsQueryable();
        }

        public async Task CacheAllAsync()
        {
            _memoryCache.Set(CacheKey, await _repository.GetAllAsync());
        }
    }
}
