using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Contexts.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace HomeAssignment.Database;

public static class DependencyInjection
{
    public static void AddDatabaseServices(this IServiceCollection services)
    {
        services.AddSingleton<IHomeworkAssignmentDbContextProvider, HomeworkAssignmentDbContextProvider>();
        
        services.AddSingleton<IHomeworkAssignmentDbContextFactory>(provider =>
        {
            var contextFactoryProvider = provider.GetService<IHomeworkAssignmentDbContextProvider>();
            return contextFactoryProvider!.GetFactory();
        });
        
        services.AddSingleton<IHomeworkAssignmentDbContext>(provider =>
        {
            var contextFactory = provider.GetService<IHomeworkAssignmentDbContextFactory>();
            return contextFactory!.CreateDbContext();
        }); 
    }
}