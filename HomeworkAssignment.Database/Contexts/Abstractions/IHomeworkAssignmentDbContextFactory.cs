using HomeAssignment.Database.Contexts.Implementations;

namespace HomeAssignment.Database.Contexts.Abstractions;

public interface IHomeworkAssignmentDbContextFactory
{
    HomeworkAssignmentDbContext CreateDbContext();
}