using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Contexts.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace HomeAssignment.Database;

public static class DependencyInjection
{
    public static void AddDatabaseServices(this IServiceCollection services)
    {
        services.AddScoped<IHomeworkAssignmentDbContextProvider, HomeworkAssignmentDbContextProvider>();

        services.AddScoped<IHomeworkAssignmentDbContextFactory>(provider =>
        {
            var contextFactoryProvider = provider.GetService<IHomeworkAssignmentDbContextProvider>();
            return contextFactoryProvider!.GetFactory();
        });

        services.AddScoped<IHomeworkAssignmentDbContext>(provider =>
        {
            var contextFactory = provider.GetService<IHomeworkAssignmentDbContextFactory>();
            return contextFactory!.CreateDbContext();
        });
    }
}