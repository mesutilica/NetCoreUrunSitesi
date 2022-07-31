using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Abstract
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetWithSlugByIdAsync(int categoryId);
        Task<IEnumerable<Product>> GetAllCategoriesBySlugsAsync();
    }
}
