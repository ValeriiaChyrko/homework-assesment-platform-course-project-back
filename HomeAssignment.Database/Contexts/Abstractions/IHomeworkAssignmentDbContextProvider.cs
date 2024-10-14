namespace HomeAssignment.Database.Contexts.Abstractions;

public interface IHomeworkAssignmentDbContextProvider
{
    IHomeworkAssignmentDbContextFactory GetFactory();
}