using HomeworkAssignment.Application.Abstractions;
using HomeworkAssignment.Application.Abstractions.AssignmentRelated;
using HomeworkAssignment.Application.Abstractions.AttemptRelated;
using HomeworkAssignment.Application.Abstractions.CategoryRelated;
using HomeworkAssignment.Application.Abstractions.ChapterRelated;
using HomeworkAssignment.Application.Abstractions.Contracts;
using HomeworkAssignment.Application.Abstractions.CourseRelated;
using HomeworkAssignment.Application.Implementations.AssignmentRelated;
using HomeworkAssignment.Application.Implementations.AttemptRelated;
using HomeworkAssignment.Application.Implementations.CategoryRelated;
using HomeworkAssignment.Application.Implementations.ChapterRelated;
using HomeworkAssignment.Application.Implementations.CourseRelated;
using HomeworkAssignment.Application.Implementations.UserRelated;
using Microsoft.Extensions.DependencyInjection;

namespace HomeworkAssignment.Application;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAttemptService, AttemptService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IChapterService, ChapterService>();
        services.AddScoped<IChapterAttachmentService, ChapterAttachmentService>();
        services.AddScoped<IChapterProgressService, ChapterProgressService>();
        services.AddScoped<IAssignmentService, AssignmentService>();
        services.AddScoped<IAssignmentProgressService, AssignmentProgressService>();
        services.AddScoped<ICourseAttachmentService, CourseAttachmentService>();
        services.AddScoped<ICourseEnrollmentService, CourseEnrollmentService>();

        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<IDatabaseTransactionManager, DatabaseTransactionManager>();

        services.AddSingleton<IPasswordHasher, PasswordHasher>();
    }
}