using Core.Entities;

namespace Service.Abstract
{
    public interface IBrandService : IService<Brand>
    {
        Task<Brand> GetBrandByProductsAsync(int id);
    }
}
