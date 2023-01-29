using DAL;
using DAL.Concrete;
using Service.Abstract;

namespace Service.Concrete
{
    public class BrandService : BrandRepository, IBrandService
    {
        public BrandService(DatabaseContext context) : base(context)
        {
        }
    }
}
