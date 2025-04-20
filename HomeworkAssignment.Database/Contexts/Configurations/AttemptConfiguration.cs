using HomeAssignment.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeAssignment.Database.Contexts.Configurations;

public class AttemptConfiguration : IEntityTypeConfiguration<AttemptEntity>
{
    public void Configure(EntityTypeBuilder<AttemptEntity> builder)
    {
        builder.HasOne(a => a.User)
            .WithMany(g => g.Attempts)
            .HasForeignKey(mr => mr.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Assignment)
            .WithMany(p => p.Attempts)
            .HasForeignKey(mr => mr.AssignmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(mr => mr.BranchName).HasMaxLength(64);

        builder.HasIndex(mr => mr.AssignmentId);
    }
}