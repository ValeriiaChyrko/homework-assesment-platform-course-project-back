using HomeAssignment.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Database.Contexts.Abstractions;

public interface IHomeworkAssignmentDbContext : IUnitOfWork
{
    DbSet<AssignmentEntity> AssignmentEntities { get; set; }
    DbSet<AttemptEntity> AttemptEntities { get; set; }
    DbSet<GitHubProfilesEntity> GitHubProfilesEntities { get; set; }
    DbSet<UserEntity> UserEntities { get; set; }
}