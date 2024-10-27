using HomeAssignment.Database.Contexts.Abstractions;
using Microsoft.Extensions.Configuration;

namespace HomeAssignment.Database.Contexts.Implementations;

public class HomeworkAssignmentDbContextProvider : IHomeworkAssignmentDbContextProvider
{
    private readonly IConfiguration _config;

    public HomeworkAssignmentDbContextProvider(IConfiguration config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    public IHomeworkAssignmentDbContextFactory GetFactory()
    {
        var dbContextType = _config["DbContext:Type"];
        if (string.IsNullOrWhiteSpace(dbContextType))
            throw new ArgumentException("DbContext type must be specified", nameof(dbContextType));

        return dbContextType switch
        {
            "PostgresSql" => new HomeworkAssignmentPostgresDbContextFactory(_config),
            _ => throw new ArgumentException("Invalid DbContext type", nameof(dbContextType))
        };
    }
}