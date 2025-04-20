using AutoMapper;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RespondDTOs.CourseRelated;
using HomeAssignment.Persistence.Commands.ChapterUserProgresses;
using HomeAssignment.Persistence.Commands.Enrollments;
using HomeAssignment.Persistence.Queries.Enrollments;
using HomeworkAssignment.Application.Abstractions;
using HomeworkAssignment.Application.Abstractions.Contracts;
using HomeworkAssignment.Application.Abstractions.CourseRelated;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HomeworkAssignment.Application.Implementations.CourseRelated;

public class CourseEnrollmentService(
    ILogger<CourseEnrollmentService> logger,
    IDatabaseTransactionManager transactionManager,
    IMediator mediator,
    IMapper mapper)
    : BaseService<CourseEnrollmentService>(logger, transactionManager), ICourseEnrollmentService
{
    private readonly ILogger<CourseEnrollmentService> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<RespondEnrollmentDto> EnrollAsync(Guid userId, Guid courseId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting enrollment process for course ID: {CourseId}", courseId);

        var existingEnrollment = await ExecuteTransactionAsync(
            async () => await mediator.Send(new GetEnrollmentByIdQuery(userId, courseId), cancellationToken),
            cancellationToken: cancellationToken);

        if (existingEnrollment != null)
        {
            _logger.LogWarning("User is already enrolled in course ID: {CourseId}", courseId);
            throw new ArgumentException("User is already enrolled.");
        }

        var enrollment = Enrollment.Create(userId, courseId);

        var addedEnrollment = await ExecuteTransactionAsync(
            async () => await mediator.Send(new CreateEnrollmentCommand(enrollment), cancellationToken),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Successfully enrolled user in course ID: {CourseId}", courseId);
        return mapper.Map<RespondEnrollmentDto>(addedEnrollment);
    }

    public async Task WithdrawAsync(Guid userId, Guid courseId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting withdrawal process for course ID: {CourseId}", courseId);

        var existingEnrollment = await ExecuteTransactionAsync(
            async () => await mediator.Send(new GetEnrollmentByIdQuery(userId, courseId), cancellationToken),
            cancellationToken: cancellationToken);

        if (existingEnrollment == null)
        {
            _logger.LogWarning("No enrollment found for course ID: {CourseId}", courseId);
            throw new ArgumentException("Enrollment does not exist.");
        }

        await ExecuteTransactionAsync(async () =>
        {
            await mediator.Send(new DeleteCourseUserProgressCommand(courseId), cancellationToken);
            await mediator.Send(new DeleteEnrollmentCommand(existingEnrollment.Id), cancellationToken);
        }, cancellationToken: cancellationToken);

        _logger.LogInformation("Successfully withdrew user from course ID: {CourseId}", courseId);
    }

    public async Task<RespondEnrollmentDto?> GetEnrollmentAsync(Guid userId, Guid courseId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving enrollment details for course ID: {CourseId}", courseId);

        var result = await ExecuteTransactionAsync(
            async () => await mediator.Send(new GetEnrollmentByIdQuery(userId, courseId), cancellationToken),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Successfully retrieved enrollment details for course ID: {CourseId}", courseId);
        return mapper.Map<RespondEnrollmentDto>(result);
    }
    
    public async Task<RespondEnrollmentsAnalyticsDto> GetEnrollmentsAnalyticsAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving enrolled courses for user {UserId}", userId);

        var enrollments = await ExecuteWithExceptionHandlingAsync(
            () => mediator.Send(new GetAllEnrollmentsByCourseOwnerIdQuery(userId), cancellationToken)
        );

        var enrollmentList = enrollments.ToList();
        if (enrollmentList.Count == 0)
        {
            _logger.LogInformation("No enrollments found for user {UserId}", userId);
            return new RespondEnrollmentsAnalyticsDto
            {
                Analysis = [],
                TotalStudents = 0
            };
        }

        var courseIds = enrollmentList.Select(e => e.CourseId).Distinct().ToList();

        var courses = await ExecuteWithExceptionHandlingAsync(
            () => mediator.Send(new GetCoursesByIdsQuery(courseIds), cancellationToken)
        );

        var courseDict = courses.ToDictionary(c => c.Id, c => c);

        var analysis = courseIds
            .Where(courseDict.ContainsKey)
            .Select(courseId => new RespondCourseAnalyticsDto
            {
                CourseId = courseDict[courseId].Id,
                CourseTitle = courseDict[courseId].Title,
                EnrollmentsAmount = enrollmentList.Count(e => e.CourseId == courseId)
            })
            .ToList();

        _logger.LogInformation("Successfully retrieved {Count} enrolled courses for user {UserId}", analysis.Count,
            userId);

        return new RespondEnrollmentsAnalyticsDto
        {
            Analysis = analysis,
            TotalStudents = enrollmentList.Count
        };
    }
}