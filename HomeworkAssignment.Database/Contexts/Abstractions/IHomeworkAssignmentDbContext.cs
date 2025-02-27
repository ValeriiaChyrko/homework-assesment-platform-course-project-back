using HomeAssignment.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Database.Contexts.Abstractions;

public interface IHomeworkAssignmentDbContext : IUnitOfWork
{
    DbSet<AssignmentEntity> AssignmentEntities { get; set; }
    DbSet<AttachmentEntity> AttachmentEntities { get; set; } 
    DbSet<AttemptProgressEntity> AttemptProgressEntities { get; set; } 
    DbSet<CategoryEntity> CategoryEntities { get; set; } 
    DbSet<ChapterEntity> ChapterEntities { get; set; } 
    DbSet<CourseEntity> CourseEntities { get; set; } 
    DbSet<EnrollmentEntity> EnrollmentEntities { get; set; }
    DbSet<MuxDataEntity> MuxDataEntities { get; set; }
    DbSet<UserEntity> UserEntities { get; set; } 
    DbSet<UserProgressEntity> UserProgressEntities { get; set; } 
}