using Core.Entities;
using DAL.Configurations;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class DatabaseContext : DbContext
    {
        //public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options){}
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(LocalDB)\MssqlLocalDB; Database=NetCoreUrunSitesi; Trusted_Connection=True; MultipleActiveResultSets=True");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //FluentApi
            modelBuilder.Entity<AppUser>().Property(a => a.Name).IsRequired().HasColumnType("varchar(50)").HasMaxLength(50);
            modelBuilder.Entity<AppUser>().Property(a => a.Surname).IsRequired().HasColumnType("varchar(50)").HasMaxLength(50);
            modelBuilder.Entity<AppUser>().Property(a => a.Username).HasColumnType("varchar(50)").HasMaxLength(50);
            modelBuilder.Entity<AppUser>().Property(a => a.Password).IsRequired().HasColumnType("nvarchar(150)").HasMaxLength(150);
            modelBuilder.Entity<AppUser>().Property(a => a.Email).IsRequired().HasColumnType("varchar(50)").HasMaxLength(50);
            modelBuilder.Entity<AppUser>().Property(a => a.Phone).HasColumnType("varchar(15)").HasMaxLength(50);
            // HasData metodu db oluştuktan sonra ilk kaydı eklemek için
            modelBuilder.Entity<AppUser>().HasData(
            new AppUser
            {
                Id = 1,
                CreateDate = DateTime.Now,
                Username = "Admin",
                Password = "123456",
                Email = "admin@admin.coo",
                Phone = "",
                Name = "Admin",
                Surname = "Admin",
                IsActive = true,
                IsAdmin = true,
                RefreshToken = Guid.NewGuid().ToString(),
                RefreshTokenExpireDate = DateTime.Now.AddMinutes(30),
            }
            );
            modelBuilder.ApplyConfiguration(new BrandConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ContactConfiguration());
            modelBuilder.ApplyConfiguration(new NewsConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new SliderConfiguration());

            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Fluent validation : FluentValidation.AspNetCore paketini yüklüyoruz

            base.OnModelCreating(modelBuilder);
        }
    }
}