using HomeAssignment.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeAssignment.Database.Contexts.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<CourseEntity>
{
    public void Configure(EntityTypeBuilder<CourseEntity> builder)
    {
        builder.HasOne(a => a.Category)
            .WithMany(p => p.Courses)
            .HasForeignKey(mr => mr.CategoryId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(a => a.Chapters)
            .WithOne(g => g.Course)
            .HasForeignKey(mr => mr.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.Attachments)
            .WithOne(g => g.Course)
            .HasForeignKey(mr => mr.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.Enrollments)
            .WithOne(g => g.Course)
            .HasForeignKey(mr => mr.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.User)
            .WithMany(p => p.Courses)
            .HasForeignKey(mr => mr.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(mr => mr.Title).IsRequired().HasMaxLength(64);
        builder.Property(mr => mr.Description).HasMaxLength(512);
        builder.Property(mr => mr.ImageUrl).HasMaxLength(256);

        builder.HasIndex(mr => mr.CategoryId);
    }
}