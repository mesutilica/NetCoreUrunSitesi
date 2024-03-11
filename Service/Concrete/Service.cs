using Core.Entities;
using DAL;
using Microsoft.EntityFrameworkCore;
using Service.Abstract;
using System.Linq.Expressions;

namespace Service.Concrete
{
    public class Service<T> : IService<T> where T : class, IEntity, new()
    {
        internal DatabaseContext _context; // içi boş context nesnesi tanımladık
        internal DbSet<T> _dbSet; // içi boş dbSet nesnesi tanımladık
        public Service(DatabaseContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public int Add(T entity)
        {
            _dbSet.Add(entity);
            return SaveChanges();
        }

        public async Task AddAsync(T entity) // asenkron metotlarda task in önüne async takısı eklenmelidir!
        {
            //await dbSet.AddAsync(entity); // asenkron metotları kullanırken await anahtar kelimesi kullanılır
            await _context.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            //dbSet.Remove(entity);
            _context.Remove(entity);
        }

        public T Find(int id)
        {
            return _dbSet.Find(id);
        }

        public async Task<T> FindAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.FirstOrDefaultAsync(expression);
        }

        public T Get(Expression<Func<T, bool>> expression)
        {
            return _dbSet.FirstOrDefault(expression);
        }

        public List<T> GetAll()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        public List<T> GetAll(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression).AsNoTracking().ToList();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.AsNoTracking().Where(expression).ToListAsync();
        }

        public IQueryable<T> GetAllInclude(string table)
        {
            return _dbSet.Include(table);
        }

        public async Task<T> GetIncludeAsync(Expression<Func<T, bool>> expression, string table)
        {
            return await _dbSet.Include(table).FirstOrDefaultAsync(expression);
        }

        public IQueryable<T> GetQueryable()
        {
            return _dbSet;
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _context.Update(entity);
        }
    }
}
