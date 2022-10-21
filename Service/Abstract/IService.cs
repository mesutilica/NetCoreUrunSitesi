using DAL.Abstract;
using Entities;

namespace Service.Abstract
{
    public interface IService<T> : IRepository<T> where T : class, IEntity, new()
    {

    }
}
