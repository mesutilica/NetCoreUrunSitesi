using DAL;
using DAL.Concrete;
using Entities;
using Service.Abstract;

namespace Service.Concrete
{
    public class Service<T> : Repository<T>, IService<T> where T : class, IEntity, new()
    {
        public Service(DatabaseContext _context) : base(_context)
        {
        }
    }
}
