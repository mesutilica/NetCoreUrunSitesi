using DAL;
using DAL.Concrete;
using Service.Abstract;

namespace Service.Concrete
{
    public class CategoryService : CategoryRepository, ICategoryService
    {
        public CategoryService(DatabaseContext context) : base(context)
        {
        }
    }
}
