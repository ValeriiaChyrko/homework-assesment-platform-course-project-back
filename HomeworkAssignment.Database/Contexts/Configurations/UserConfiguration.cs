using HomeAssignment.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeAssignment.Database.Contexts.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder
            .HasMany(a => a.Courses)
            .WithOne(u => u.User)
            .HasForeignKey(u => u.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(a => a.Attempts)
            .WithOne(g => g.User)
            .HasForeignKey(mr => mr.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.UsersProgress)
            .WithOne(g => g.User)
            .HasForeignKey(mr => mr.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.Enrollments)
            .WithOne(g => g.User)
            .HasForeignKey(mr => mr.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(mr => mr.Email);

        builder.Property(mr => mr.FullName).IsRequired().HasMaxLength(128);
        builder.Property(mr => mr.Email).IsRequired().HasMaxLength(254);
        builder.Property(mr => mr.PasswordHash).IsRequired().HasMaxLength(254);
        builder.Property(mr => mr.RoleType).HasMaxLength(32);

        builder.Property(mr => mr.GithubUsername).IsRequired().HasMaxLength(64);
        builder.Property(mr => mr.GithubProfileUrl).IsRequired().HasMaxLength(128);
        builder.Property(mr => mr.GithubPictureUrl).HasMaxLength(128);

        builder.HasIndex(mr => mr.Email).IsUnique();
        builder.HasIndex(mr => mr.GithubUsername).IsUnique();
    }
}