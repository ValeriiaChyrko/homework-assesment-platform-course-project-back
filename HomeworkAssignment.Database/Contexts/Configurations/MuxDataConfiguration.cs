using HomeAssignment.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeAssignment.Database.Contexts.Configurations;

public class MuxDataConfiguration : IEntityTypeConfiguration<MuxDataEntity>
{
    public void Configure(EntityTypeBuilder<MuxDataEntity> builder)
    {
        builder.HasOne(a => a.Chapter)
            .WithOne(g => g.MuxData)
            .HasForeignKey<MuxDataEntity>(mr => mr.ChapterId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.Property(mr => mr.AssetId).HasMaxLength(256);
        builder.Property(mr => mr.PlaybackId).HasMaxLength(256);
        builder.HasIndex(mr => mr.PlaybackId).IsUnique();
    }
}