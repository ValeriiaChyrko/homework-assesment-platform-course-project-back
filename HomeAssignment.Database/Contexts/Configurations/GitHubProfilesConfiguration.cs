using HomeAssignment.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeAssignment.Database.Contexts.Configurations;

public class GitHubProfilesConfiguration : IEntityTypeConfiguration<GitHubProfilesEntity>
{
    public void Configure(EntityTypeBuilder<GitHubProfilesEntity> builder)
    {
        builder.HasMany(a => a.Attempts)
            .WithOne(g => g.Student)
            .HasForeignKey(mr => mr.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(mr => mr.GithubUsername).IsRequired().HasMaxLength(64);
        builder.Property(mr => mr.GithubAccessToken).IsRequired().HasMaxLength(128);
        builder.Property(mr => mr.GithubProfileUrl).IsRequired().HasMaxLength(128);
        builder.Property(mr => mr.GithubPictureUrl).HasMaxLength(128);
    }
}