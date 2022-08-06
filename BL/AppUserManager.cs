using DAL;

namespace BL
{
    public class AppUserManager : Repository<Entities.AppUser>, IRepository<Entities.AppUser>
    {
        public AppUserManager(DatabaseContext _context) : base(_context)
        {
        }
    }
}
