using HomeAssignment.Domain.Abstractions.Contracts;
using HomeAssignment.Domain.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace HomeAssignment.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddSingleton<ILoggerFactoryProvider, LoggerFactoryProvider>();
        services.AddSingleton<ILoggerFactory>(provider =>
        {
            var loggerFactoryProvider = provider.GetService<ILoggerFactoryProvider>();
            return loggerFactoryProvider!.GetFactory();
        });
        services.AddSingleton<ILogger>(provider =>
        {
            var loggerFactory = provider.GetService<ILoggerFactory>();
            return loggerFactory!.CreateLogger();
        });

        return services;
    }
}