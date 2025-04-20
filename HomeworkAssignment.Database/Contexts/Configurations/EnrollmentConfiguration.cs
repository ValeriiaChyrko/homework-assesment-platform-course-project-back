using HomeAssignment.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeAssignment.Database.Contexts.Configurations;

public class EnrollmentConfiguration : IEntityTypeConfiguration<EnrollmentEntity>
{
    public void Configure(EntityTypeBuilder<EnrollmentEntity> builder)
    {
        builder.HasOne(a => a.Course)
            .WithMany(g => g.Enrollments)
            .HasForeignKey(mr => mr.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.User)
            .WithMany(g => g.Enrollments)
            .HasForeignKey(mr => mr.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(mr => mr.CourseId);
        builder.HasIndex(u => new { u.UserId, u.CourseId }).IsUnique();
    }
}