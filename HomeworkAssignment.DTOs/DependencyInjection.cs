using System.Reflection;
using FluentValidation;
using HomeAssignment.DTOs.MappingProfiles;
using Microsoft.Extensions.DependencyInjection;

namespace HomeAssignment.DTOs;

public static class DependencyInjection
{
    public static void AddDtosServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<AssignmentMappingProfile>();
            cfg.AddProfile<AttachmentMappingProfile>();
            cfg.AddProfile<AttemptMappingProfile>();
            cfg.AddProfile<CategoryMappingProfile>();
            cfg.AddProfile<CourseMappingProfile>();
            cfg.AddProfile<ChapterMappingProfile>();
            cfg.AddProfile<EnrollmentMappingProfile>();
            cfg.AddProfile<MuxDataMappingProfile>();
            cfg.AddProfile<UserMappingProfile>();
            cfg.AddProfile<UserProgressMappingProfile>();
        });
    }
}