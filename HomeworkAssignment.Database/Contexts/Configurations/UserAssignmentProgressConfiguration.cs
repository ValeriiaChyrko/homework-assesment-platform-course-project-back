using HomeAssignment.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeAssignment.Database.Contexts.Configurations;

public class UserAssignmentProgressConfiguration : IEntityTypeConfiguration<UserAssignmentProgressEntity>
{
    public void Configure(EntityTypeBuilder<UserAssignmentProgressEntity> builder)
    {
        builder
            .HasOne(up => up.User)
            .WithMany()
            .HasForeignKey(up => up.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(up => up.Assignment)
            .WithMany(a => a.UsersProgress) 
            .HasForeignKey(up => up.AssignmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}