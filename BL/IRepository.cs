using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions; //
using System.Threading.Tasks;

namespace BL
{
    public interface IRepository<T> where T : class
    {
        List<T> GetAll();
        List<T> GetAll(Expression<Func<T, bool>> expression);
        T Get(Expression<Func<T, bool>> expression);
        T Find(int id);
        int Add(T entity);
        int Update(T entity);
        int Delete(T entity);
        int SaveChanges();
        IQueryable<T> GetAllInclude(string table);
        IQueryable<T> GetAllInclude(Expression<Func<T, bool>> expression);

        //Asenkron metotlar
        Task<T> FindAsync(int id);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression);
        IQueryable<T> FindAllAsync(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression);
        Task AddAsync(T entity);
        Task<int> SaveChangesAsync();
    }
}
