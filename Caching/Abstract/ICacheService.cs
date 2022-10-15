using System.Linq.Expressions;

namespace Caching.Abstract
{
    public interface ICacheService<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        IQueryable<T> GetAllQueryable(Expression<Func<T, bool>> expression);
    }
}
