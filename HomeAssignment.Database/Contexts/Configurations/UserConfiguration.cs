using HomeAssignment.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeAssignment.Database.Contexts.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder
            .HasMany(a => a.GitHubProfiles)
            .WithOne(u => u.User)
            .HasForeignKey(u => u.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(mr => mr.Email);
        
        builder.Property(mr => mr.FullName).IsRequired().HasMaxLength(128);
        builder.Property(mr => mr.Email).IsRequired().HasMaxLength(254);
        builder.Property(mr => mr.PasswordHash).IsRequired().HasMaxLength(254);
        builder.Property(mr => mr.RoleType).HasMaxLength(32);
    }
}