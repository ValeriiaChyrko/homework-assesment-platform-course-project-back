using AutoMapper;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RespondDTOs.CourseRelated;
using HomeAssignment.Persistence.Commands.ChapterUserProgresses;
using HomeAssignment.Persistence.Commands.Enrollments;
using HomeAssignment.Persistence.Queries.Courses;
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
        _logger.LogInformation("Enrolling in course. Course Id: {CourseId}", courseId);

        var existingEnrollment = await ExecuteTransactionAsync(
            async () => await mediator.Send(new GetEnrollmentByIdQuery(userId, courseId), cancellationToken),
            cancellationToken: cancellationToken);

        if (existingEnrollment != null)
        {
            _logger.LogWarning("User  is already enrolled in the course. Course Id: {CourseId}", courseId);
            throw new ArgumentException("User  is already enrolled.");
        }

        var enrollment = Enrollment.Create(userId, courseId);

        var addedEnrollment = await ExecuteTransactionAsync(
            async () => await mediator.Send(new CreateEnrollmentCommand(enrollment), cancellationToken),
            cancellationToken: cancellationToken);

        _logger.LogInformation("User  enrolled in the course. Course Id: {CourseId}", courseId);
        return mapper.Map<RespondEnrollmentDto>(addedEnrollment);
    }

    public async Task WithdrawAsync(Guid userId, Guid courseId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Withdrawing from course. Course Id: {CourseId}", courseId);

        var existingEnrollment = await ExecuteTransactionAsync(
            async () => await mediator.Send(new GetEnrollmentByIdQuery(userId, courseId), cancellationToken),
            cancellationToken: cancellationToken);

        if (existingEnrollment == null)
        {
            _logger.LogWarning("Enrollment not found. Course Id: {CourseId}", courseId);
            throw new ArgumentException("Enrollment does not exist.");
        }

        await ExecuteTransactionAsync(async () =>
        {
            await mediator.Send(new DeleteCourseUserProgressCommand(courseId), cancellationToken);
            await mediator.Send(new DeleteEnrollmentCommand(existingEnrollment.Id), cancellationToken);
        }, cancellationToken: cancellationToken);

        _logger.LogInformation("User  withdrawn from the course. Course Id: {CourseId}", courseId);
    }

    public async Task<RespondEnrollmentDto?> GetEnrollmentAsync(Guid userId, Guid courseId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving enrollment data for course. Course Id: {CourseId}", courseId);

        var result = await ExecuteTransactionAsync(
            async () => await mediator.Send(new GetEnrollmentByIdQuery(userId, courseId), cancellationToken),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Enrollment data retrieved for course. Course Id: {CourseId}", courseId);
        return mapper.Map<RespondEnrollmentDto>(result);
    }

    public async Task<RespondEnrollmentsAnalyticsDto> GetEnrollmentsAnalyticsAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving enrollment analytics for the course owner.");

        var enrollments = await ExecuteWithExceptionHandlingAsync(
            () => mediator.Send(new GetAllEnrollmentsByCourseOwnerIdQuery(userId), cancellationToken)
        );

        var enrollmentList = enrollments.ToList();
        if (enrollmentList.Count == 0)
        {
            _logger.LogInformation("No analytics found. No enrollments.");
            return new RespondEnrollmentsAnalyticsDto
            {
                Analysis = new List<RespondCourseAnalyticsDto>(),
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

        _logger.LogInformation("Analytics retrieved for {Count} courses", analysis.Count);

        return new RespondEnrollmentsAnalyticsDto
        {
            Analysis = analysis,
            TotalStudents = enrollmentList.Count
        };
    }
}