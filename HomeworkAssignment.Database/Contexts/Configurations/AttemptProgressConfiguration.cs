using HomeAssignment.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeAssignment.Database.Contexts.Configurations;

public class AttemptProgressConfiguration : IEntityTypeConfiguration<AttemptProgressEntity>
{
    public void Configure(EntityTypeBuilder<AttemptProgressEntity> builder)
    {
        builder.HasOne(a => a.User)
            .WithMany(g => g.Attempts)
            .HasForeignKey(mr => mr.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(a => a.Assignment)
            .WithMany(p => p.Attempts)
            .HasForeignKey(mr => mr.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(mr => mr.BranchName).IsRequired().HasMaxLength(64);
        builder.Property(mr => mr.ProgressStatus).HasMaxLength(64);
        
        builder.HasIndex(mr => mr.AssignmentId);
    }
}