using HomeAssignment.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeAssignment.Database.Contexts.Configurations;

public class UserChapterProgressConfiguration : IEntityTypeConfiguration<UserChapterProgressEntity>
{
    public void Configure(EntityTypeBuilder<UserChapterProgressEntity> builder)
    {
        builder
            .HasOne(a => a.User)
            .WithMany(u => u.UsersProgress)
            .HasForeignKey(u => u.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(mr => mr.ChapterId);
        builder.HasIndex(u => new { u.UserId, u.ChapterId }).IsUnique();
    }
}