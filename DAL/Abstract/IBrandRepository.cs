using Entities;

namespace DAL.Abstract
{
    public interface IBrandRepository : IRepository<Brand>
    {
        Task<Brand> GetBrandByProductsAsync(int id);
    }
}
