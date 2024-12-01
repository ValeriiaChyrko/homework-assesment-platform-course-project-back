using System.Reflection;
using FluentValidation;
using HomeAssignment.Persistence.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace HomeAssignment.Persistence;

public static class DependencyInjection
{
    public static void AddPersistenceServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()); });
        
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(DatabaseErrorBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }
}