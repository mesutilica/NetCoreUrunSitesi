using BL.Abstract;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Concrete
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public Task<IEnumerable<Product>> GetAllCategoriesBySlugsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetWithSlugByIdAsync(int categoryId)
        {
            throw new NotImplementedException();
        }
    }
}
