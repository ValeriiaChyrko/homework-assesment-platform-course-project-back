using HomeAssignment.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeAssignment.Database.Contexts.Configurations;

public class ChapterConfiguration : IEntityTypeConfiguration<ChapterEntity>
{
    public void Configure(EntityTypeBuilder<ChapterEntity> builder)
    {
        builder.HasOne(a => a.Course)
            .WithMany(p => p.Chapters)
            .HasForeignKey(mr => mr.CourseId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(a => a.UsersProgress)
            .WithOne(g => g.Chapter)
            .HasForeignKey(mr => mr.ChapterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.Attachments)
            .WithOne(g => g.Chapter)
            .HasForeignKey(mr => mr.ChapterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.Assignments)
            .WithOne(g => g.Chapter)
            .HasForeignKey(mr => mr.ChapterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(mr => mr.Title).IsRequired().HasMaxLength(64);
        builder.Property(mr => mr.Description).HasMaxLength(10000);
        builder.Property(mr => mr.VideoUrl).HasMaxLength(256);

        builder.HasIndex(mr => mr.CourseId);
    }
}