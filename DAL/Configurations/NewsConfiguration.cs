using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    internal class NewsConfiguration : IEntityTypeConfiguration<News>
    {
        public void Configure(EntityTypeBuilder<News> builder)
        {
            builder.Property(x => x.Image).IsRequired().HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.Image).IsRequired().HasMaxLength(100);
        }
    }
}
