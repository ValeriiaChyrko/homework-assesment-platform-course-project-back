using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace HomeAssignment.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()); });

        return services;
    }
}