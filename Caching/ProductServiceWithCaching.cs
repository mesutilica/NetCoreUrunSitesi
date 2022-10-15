using BL.Abstract;
using Entities;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Expressions;

namespace Caching
{
    public class ProductServiceWithCaching : IProductService
    {
        private const string CacheProductKey = "productsCache";
        private readonly IMemoryCache _memoryCache;
        private readonly IRepository<Product> _repository;
        public int Add(Product entity)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(Product entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Product entity)
        {
            throw new NotImplementedException();
        }

        public Product Find(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Product> FindAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Product> FirstOrDefaultAsync(Expression<Func<Product, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Product Get(Expression<Func<Product, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<Product> GetAll(Expression<Func<Product, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllAsync(Expression<Func<Product, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllCategoriesByProductsAsync()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Product> GetAllInclude(string table)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetWithCategoryByIdAsync(int categoryId)
        {
            throw new NotImplementedException();
        }

        public int SaveChanges()
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public void Update(Product entity)
        {
            throw new NotImplementedException();
        }
    }
}