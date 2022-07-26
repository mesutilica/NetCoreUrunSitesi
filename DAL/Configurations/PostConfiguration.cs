﻿using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    internal class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.Image).HasColumnType("varchar(100)").HasMaxLength(100);
        }
    }
}
