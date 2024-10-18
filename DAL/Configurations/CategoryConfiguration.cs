using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Image).HasMaxLength(100);
            builder.HasData(new Category()
            {
                Id = 1,
                CreateDate = DateTime.Now,
                IsActive = true,
                IsTopMenu = true,
                Name = "Elektronik",
                OrderNo = 1,
                ParentId = 0
            },
            new Category()
            {
                Id = 2,
                CreateDate = DateTime.Now,
                IsActive = true,
                IsTopMenu = true,
                Name = "Bilgisayar",
                OrderNo = 2,
                ParentId = 0
            },
            new Category()
            {
                Id = 3,
                CreateDate = DateTime.Now,
                IsActive = true,
                IsTopMenu = true,
                Name = "Telefon",
                OrderNo = 3,
                ParentId = 0
            });
        }
    }
}
