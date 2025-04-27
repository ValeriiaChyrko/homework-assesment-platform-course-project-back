using HomeAssignment.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeAssignment.Database.Contexts.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<CategoryEntity>
{
    public void Configure(EntityTypeBuilder<CategoryEntity> builder)
    {
        builder.HasMany(a => a.Courses)
            .WithOne(g => g.Category)
            .HasForeignKey(mr => mr.CategoryId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(mr => mr.Name).IsRequired().HasMaxLength(64);
        builder.HasIndex(mr => mr.Name).IsUnique();
    }
}