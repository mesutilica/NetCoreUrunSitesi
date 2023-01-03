using DAL;
using DAL.Concrete;
using Microsoft.Extensions.Caching.Memory;
using Service.Abstract;

namespace Service.Concrete
{
    public class ProductService : ProductRepository, IProductService
    {        
        public ProductService(DatabaseContext context, IMemoryCache memoryCache) : base(context, memoryCache)
        {
        }
    }
}
