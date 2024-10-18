using Core.Entities;
using Data;
using Microsoft.EntityFrameworkCore;
using Service.Abstract;

namespace Service.Concrete
{
    public class BrandService : Service<Brand>, IBrandService
    {
        public BrandService(DatabaseContext context) : base(context)
        {
        }
        public async Task<Brand> GetBrandByProductsAsync(int id)
        {
            return await _dbSet.Where(c => c.Id == id).AsNoTracking().Include(c => c.Products).FirstOrDefaultAsync();
        }
    }
}
