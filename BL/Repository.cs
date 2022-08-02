using DAL;
using Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BL
{
    public class Repository<T> : IRepository<T> where T : class, IEntity, new()
    {
        public DatabaseContext context; // içi boş context nesnesi tanımladık
        DbSet<T> dbSet; // içi boş dbSet nesnesi tanımladık
        public Repository()
        {

        }
        public Repository(DatabaseContext _context)
        {
            //if (context == null)
            //{
            //    context = new DatabaseContext(); // uygulama çalışınca context boşsa doldur
            //    dbSet = context.Set<T>(); // dbSet nesnesini de oluşturulan context içinde bize gelen classa göre ayarla
            //}
            context = _context;
            dbSet = context.Set<T>();
        }
        public int Add(T entity)
        {
            dbSet.Add(entity);
            return SaveChanges();
        }

        public async Task AddAsync(T entity) // asenkron metotlarda task in önüne async takısı eklenmelidir!
        {
            await dbSet.AddAsync(entity); // asemkron metotları kullanırken await anahtar kelimesi kullanılır
        }

        public int Delete(T entity)
        {
            dbSet.Remove(entity);
            return SaveChanges();
        }

        public T Find(int id)
        {
            return dbSet.Find(id);
        }

        public IQueryable<T> FindAllAsync(Expression<Func<T, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public async Task<T> FindAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
            return await dbSet.FirstOrDefaultAsync(expression);
        }

        public T Get(Expression<Func<T, bool>> expression)
        {
            return dbSet.FirstOrDefault(expression);
        }

        public List<T> GetAll()
        {
            return dbSet.ToList();
        }

        public List<T> GetAll(Expression<Func<T, bool>> expression)
        {
            return dbSet.Where(expression).ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression)
        {
            return await dbSet.Where(expression).ToListAsync();
        }

        public IQueryable<T> GetAllInclude(string table)
        {
            return dbSet.Include(table);
        }

        public IQueryable<T> GetAllInclude(Expression<Func<T, bool>> expression)
        {
            return dbSet.Where(expression);
        }

        public int SaveChanges()
        {
            return context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

        public int Update(T entity)
        {
            context.Update(entity);
            return SaveChanges();
        }
    }
}
