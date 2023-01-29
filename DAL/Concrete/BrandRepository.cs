using DAL.Abstract;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Concrete
{
    public class BrandRepository : Repository<Brand>, IBrandRepository
    {
        public BrandRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<Brand> GetBrandByProductsAsync(int id)
        {
            return await _dbSet.Where(c => c.Id == id).AsNoTracking().Include(c => c.Products).FirstOrDefaultAsync();
        }
    }
}
