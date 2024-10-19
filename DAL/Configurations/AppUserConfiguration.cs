using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    internal class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasColumnType("varchar(50)").HasMaxLength(50);
            builder.Property(x => x.Surname).IsRequired().HasColumnType("varchar(50)").HasMaxLength(50);
            builder.Property(x => x.Username).HasColumnType("varchar(50)").HasMaxLength(50);
            builder.Property(x => x.Password).IsRequired().HasColumnType("nvarchar(150)").HasMaxLength(150);
            builder.Property(x => x.Email).IsRequired().HasColumnType("varchar(50)").HasMaxLength(50);
            builder.Property(x => x.Phone).HasColumnType("varchar(15)").HasMaxLength(50);
            builder.HasData(
            new AppUser
            {
                Id = 1,
                CreateDate = DateTime.Now,
                Username = "Admin",
                Password = "123456",
                Email = "admin@eticaret.io",
                Name = "Admin",
                Surname = "User",
                IsActive = true,
                IsAdmin = true,
                RefreshToken = Guid.NewGuid().ToString(),
                RefreshTokenExpireDate = DateTime.Now.AddMinutes(30),
            }
            );
        }
    }
}
