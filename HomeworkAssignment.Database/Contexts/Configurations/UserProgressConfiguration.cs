using HomeAssignment.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeAssignment.Database.Contexts.Configurations;

public class UserProgressConfiguration : IEntityTypeConfiguration<UserProgressEntity>
{
    public void Configure(EntityTypeBuilder<UserProgressEntity> builder)
    {
        builder
            .HasOne(a => a.User)
            .WithMany(u => u.UsersProgress)
            .HasForeignKey(u => u.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder
            .HasOne(a => a.Chapter)
            .WithMany(u => u.UsersProgress)
            .HasForeignKey(u => u.ChapterId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasIndex(mr => mr.ChapterId);
        builder.HasIndex(u => new { u.UserId, u.ChapterId }).IsUnique();
    }
}