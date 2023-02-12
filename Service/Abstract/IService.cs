using Core.Entities;
using DAL.Abstract;

namespace Service.Abstract
{
    public interface IService<T> : IRepository<T> where T : class, IEntity, new()
    {

    }
}
