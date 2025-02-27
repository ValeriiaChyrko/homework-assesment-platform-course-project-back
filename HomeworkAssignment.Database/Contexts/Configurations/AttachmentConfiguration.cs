using HomeAssignment.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeAssignment.Database.Contexts.Configurations;

public class AttachmentConfiguration : IEntityTypeConfiguration<AttachmentEntity>
{
    public void Configure(EntityTypeBuilder<AttachmentEntity> builder)
    {
        builder.HasOne(a => a.Course)
            .WithMany(g => g.Attachments)
            .HasForeignKey(mr => mr.CourseId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(a => a.Chapter)
            .WithMany(p => p.Attachments)
            .HasForeignKey(mr => mr.ChapterId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(mr => mr.Name).IsRequired().HasMaxLength(64);
        builder.Property(mr => mr.Url).HasMaxLength(512);
        
        builder.HasIndex(mr => mr.CourseId);
    }
}