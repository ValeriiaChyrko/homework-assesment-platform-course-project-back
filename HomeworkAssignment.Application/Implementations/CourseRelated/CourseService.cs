using AutoMapper;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RequestDTOs.CourseRelated;
using HomeAssignment.DTOs.RespondDTOs.ChapterRelated;
using HomeAssignment.DTOs.RespondDTOs.CourseRelated;
using HomeAssignment.Persistence.Abstractions;
using HomeAssignment.Persistence.Commands.Courses;
using HomeAssignment.Persistence.Queries.Chapters;
using HomeAssignment.Persistence.Queries.Courses;
using HomeAssignment.Persistence.Queries.Enrollments;
using HomeAssignment.Persistence.Queries.UserChapterProgresses;
using HomeworkAssignment.Application.Abstractions;
using HomeworkAssignment.Application.Abstractions.Contracts;
using HomeworkAssignment.Application.Abstractions.CourseRelated;
using MediatR;
using Microsoft.Extensions.Logging;
using Guid = System.Guid;

namespace HomeworkAssignment.Application.Implementations.CourseRelated;

public class CourseService(
    ILogger<CourseService> logger,
    IDatabaseTransactionManager transactionManager,
    IMediator mediator,
    IMapper mapper)
    : BaseService<CourseService>(logger, transactionManager), ICourseService
{
    private readonly ILogger<CourseService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<RespondCourseDto> CreateCourseAsync(Guid userId, RequestCreateCourseDto createCourseDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating course. Data: {@CourseDto}", createCourseDto);

        var course = mapper.Map<Course>(createCourseDto);
        course.UserId = userId;

        course = await ExecuteTransactionAsync(
            () => mediator.Send(new CreateCourseCommand(course), cancellationToken),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Course created. Id: {CourseId}", course.Id);
        return mapper.Map<RespondCourseDto>(course);
    }

    public async Task<RespondCourseDto> UpdateCourseAsync(Guid userId, Guid courseId, RequestPartialCourseDto courseDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating course. Id: {CourseId}", courseId);

        var course = await mediator.Send(new GetCourseByOwnerIdQuery(courseId, userId), cancellationToken);
        if (course == null)
        {
            _logger.LogWarning("Course not found. Id: {CourseId}", courseId);
            throw new ArgumentException("Course does not exist.");
        }

        course.PatchUpdate(courseDto.Title, courseDto.Description, courseDto.ImageUrl,
            courseDto.CategoryId ?? course.CategoryId);

        var updatedCourse = await ExecuteTransactionAsync(
            () => mediator.Send(new UpdateCourseCommand(courseId, course), cancellationToken),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Course updated. Id: {CourseId}", updatedCourse.Id);
        return mapper.Map<RespondCourseDto>(updatedCourse);
    }

    public async Task DeleteCourseAsync(Guid userId, Guid courseId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting course. Id: {CourseId}", courseId);

        await ExecuteTransactionAsync(
            () => mediator.Send(new DeleteCourseCommand(courseId), cancellationToken),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Course deleted. Id: {CourseId}", courseId);
    }

    public async Task<RespondCourseDto> PublishCourseAsync(Guid userId, Guid courseId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Publishing course. Id: {CourseId}", courseId);

        var hasPublishedChapters =
            await mediator.Send(new IsAnyPublishedChapterByCourseIdQuery(courseId), cancellationToken);
        if (!hasPublishedChapters)
        {
            _logger.LogInformation("Course cannot be published. No published chapters. Id: {CourseId}",
                courseId);
            throw new ArgumentException("Course does not have published chapters.");
        }

        var course = await mediator.Send(new GetCourseByOwnerIdQuery(courseId, userId), cancellationToken);
        if (course == null)
        {
            _logger.LogWarning("Course not found. Id: {CourseId}", courseId);
            throw new ArgumentException("Course does not exist.");
        }

        course.MarkAsPublished();

        var updatedCourse = await ExecuteTransactionAsync(
            () => mediator.Send(new UpdateCourseCommand(courseId, course), cancellationToken),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Course published. Id: {CourseId}", updatedCourse.Id);
        return mapper.Map<RespondCourseDto>(updatedCourse);
    }

    public async Task<RespondCourseDto> UnpublishCourseAsync(Guid userId, Guid courseId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Unpublishing course. Id: {CourseId}", courseId);

        var course = await mediator.Send(new GetCourseByOwnerIdQuery(courseId, userId), cancellationToken);
        if (course == null)
        {
            _logger.LogWarning("Course not found. Id: {CourseId}", courseId);
            throw new ArgumentException("Course does not exist.");
        }

        course.MarkAsUnpublished();

        var updatedCourse = await ExecuteTransactionAsync(
            () => mediator.Send(new UpdateCourseCommand(courseId, course), cancellationToken),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Course unpublished. Id: {CourseId}", updatedCourse.Id);
        return mapper.Map<RespondCourseDto>(updatedCourse);
    }

    public async Task<PagedList<RespondCourseFullInfoDto>> GetCoursesFullInfoAsync(
        RequestCourseFilterParameters filterParameters, Guid userId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving all courses with filter: {@Filter}", filterParameters);

        var query = new GetAllCourseDetailViewsQuery(filterParameters, userId);
        var result = await mediator.Send(query, cancellationToken);

        var mappedItems = new List<RespondCourseFullInfoDto>();

        if (filterParameters.IncludeStudentProgress)
            foreach (var course in result.Items)
            {
                var progress = await mediator.Send(new GetUserProgressPercentageQuery(userId, course.Id),
                    cancellationToken);
                var dto = mapper.Map<RespondCourseFullInfoDto>(course);
                dto.Progress = progress;
                mappedItems.Add(dto);
            }
        else
            mappedItems = result.Items.Select(mapper.Map<RespondCourseFullInfoDto>).ToList();

        _logger.LogInformation("Retrieved {Count} courses", mappedItems.Count);

        return new PagedList<RespondCourseFullInfoDto>(mappedItems, result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<PagedList<RespondCourseFullInfoDto>> GetUserCoursesFullInfoAsync(
        RequestCourseFilterParameters filterParameters, Guid userId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving courses by author.");

        var query = new GetAllCourseDetailViewsByOwnerIdQuery(filterParameters, userId);
        var result = await mediator.Send(query, cancellationToken);

        var mappedItems = result.Items.Select(mapper.Map<RespondCourseFullInfoDto>).ToList();

        _logger.LogInformation("Retrieved {Count} courses", mappedItems.Count);

        return new PagedList<RespondCourseFullInfoDto>(mappedItems, result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<RespondCourseFullInfoDto?> GetSingleCourseFullInfoAsync(
        RequestCourseFilterParameters filterParameters, Guid userId, Guid courseId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving course. Id: {CourseId}", courseId);

        var query = new GetSingleCourseDetailViewByOwnerIdQuery(filterParameters, userId, courseId);
        var result = await mediator.Send(query, cancellationToken);

        if (result == null) return null;

        if (filterParameters.IncludeChapters && result.Chapters.Count > 1)
            result.Chapters = result.Chapters.OrderBy(ch => ch.Position).ToList();

        _logger.LogInformation("Retrieved course. Id: {CourseId}", courseId);

        return mapper.Map<RespondCourseFullInfoDto>(result);
    }

    public async Task<RespondCourseFullInfoDto?> GetSingleCourseFullInfoByStudentIdAsync(
        RequestCourseFilterParameters filterParameters, Guid userId, Guid courseId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving course for student. Id: {CourseId}", courseId);

        var query = new GetSingleCourseDetailViewByStudentIdQuery(filterParameters, userId, courseId);
        var result = await mediator.Send(query, cancellationToken);

        if (result == null) return null;

        if (filterParameters.IncludeChapters && result.Chapters.Count > 1)
            result.Chapters = result.Chapters.OrderBy(ch => ch.Position).ToList();

        var dto = mapper.Map<RespondCourseFullInfoDto>(result);

        if (filterParameters.IncludeStudentProgress)
        {
            var enrollment = await mediator.Send(new GetEnrollmentByIdQuery(userId, courseId), cancellationToken);
            dto.IsEnrolled = enrollment != null;

            var progress = await mediator.Send(new GetUserProgressPercentageQuery(userId, courseId), cancellationToken);
            dto.Progress = progress;
        }

        if (query.FilterParameters is { IncludeUserProgress: true, IncludeChapters: true } && result.Chapters.Any())
            foreach (var chapter in dto.Chapters!)
            {
                var chapterProgress = await mediator.Send(new GetUserChapterProgressByIdQuery(userId, chapter.Id),
                    cancellationToken);
                chapter.UserProgress = chapterProgress == null
                    ? null
                    : mapper.Map<RespondChapterUserProgressDto>(chapterProgress);
            }

        _logger.LogInformation("Retrieved course for student. Id : {CourseId}", courseId);

        return dto;
    }
}