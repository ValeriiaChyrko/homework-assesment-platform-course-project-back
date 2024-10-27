using HomeworkAssignment.Application.Abstractions.Contracts;
using HomeworkAssignment.Application.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace HomeworkAssignment.Application;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<IAssignmentService, AssignmentService>();
        services.AddSingleton<IAttemptService, AttemptService>();
        services.AddSingleton<IStudentService, StudentService>();
        services.AddSingleton<ITeacherService, TeacherService>();

        services.AddScoped<IDatabaseTransactionManager, DatabaseTransactionManager>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
    }
}