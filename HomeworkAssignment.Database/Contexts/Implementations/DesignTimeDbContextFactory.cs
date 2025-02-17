using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace HomeAssignment.Database.Contexts.Implementations;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<HomeworkAssignmentDbContext>
{
    public HomeworkAssignmentDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<HomeworkAssignmentDbContext>();
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        optionsBuilder.UseOpenIddict();

        return new HomeworkAssignmentDbContext(optionsBuilder.Options);
    }
}