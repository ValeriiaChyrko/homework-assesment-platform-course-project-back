using HomeAssignment.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeAssignment.Database.Contexts.Configurations;

public class AssignmentConfiguration : IEntityTypeConfiguration<AssignmentEntity>
{
    public void Configure(EntityTypeBuilder<AssignmentEntity> builder)
    {
        builder.HasOne(a => a.Chapter)
            .WithMany(p => p.Assignments)
            .HasForeignKey(mr => mr.ChapterId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(a => a.Attempts)
            .WithOne(g => g.Assignment)
            .HasForeignKey(mr => mr.AssignmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(mr => mr.Title).IsRequired().HasMaxLength(64);
        builder.Property(mr => mr.Description).HasMaxLength(512);
        builder.Property(mr => mr.RepositoryName).IsRequired().HasMaxLength(64);
        builder.Property(mr => mr.RepositoryOwner).IsRequired().HasMaxLength(64);
        builder.Property(mr => mr.RepositoryUrl).HasMaxLength(256);
        
        builder.HasIndex(mr => mr.ChapterId);
    }
}