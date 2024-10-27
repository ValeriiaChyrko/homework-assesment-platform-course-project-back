using System.Reflection;
using FluentValidation;
using HomeAssignment.DTOs.MappingProfiles;
using Microsoft.Extensions.DependencyInjection;

namespace HomeAssignment.DTOs;

public static class DependencyInjection
{
    public static IServiceCollection AddDtosServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
            cfg.AddProfile<AssignmentMappingProfile>();
            cfg.AddProfile<AttemptMappingProfile>();
            cfg.AddProfile<StudentMappingProfile>();
            cfg.AddProfile<TeacherMappingProfile>();
        });

        return services;
    }
}