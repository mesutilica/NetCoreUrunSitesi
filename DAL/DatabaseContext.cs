using Microsoft.EntityFrameworkCore;
using Entities;

namespace DAL
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {

        }
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(LocalDB)\MssqlLocalDB; Database=NetCoreUrunSitesi; Trusted_Connection=True; MultipleActiveResultSets=True");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>().HasData( // HasData metodu db oluştuktan sonra ilk kaydı eklemek için
            new AppUser
            {
                Id = 1,
                CreateDate = DateTime.Now,
                Username = "Admin",
                Password = "123456",
                IsActive = true,
                IsAdmin = true
            }
            );
            base.OnModelCreating(modelBuilder);
        }
    }
}