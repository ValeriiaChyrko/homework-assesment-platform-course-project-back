namespace HomeworkAssignment.Infrastructure.Abstractions;

public interface IBuildService
{
    Task<bool> BuildProject(string projectFile);
}