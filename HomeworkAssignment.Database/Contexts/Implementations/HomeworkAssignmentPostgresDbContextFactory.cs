using HomeAssignment.Database.Contexts.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HomeAssignment.Database.Contexts.Implementations;

public class HomeworkAssignmentPostgresDbContextFactory : IHomeworkAssignmentDbContextFactory
{
    private readonly IConfiguration _config;

    public HomeworkAssignmentPostgresDbContextFactory(IConfiguration config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    public HomeworkAssignmentDbContext CreateDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<HomeworkAssignmentDbContext>();
        optionsBuilder.UseNpgsql(_config.GetConnectionString("DefaultConnection"));
        optionsBuilder.UseOpenIddict();

        return new HomeworkAssignmentDbContext(optionsBuilder.Options);
    }
}