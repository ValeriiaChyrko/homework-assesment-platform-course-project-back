using AutoMapper;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RequestDTOs.CourseRelated;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.Persistence.Abstractions;
using HomeAssignment.Persistence.Commands.Courses;
using HomeAssignment.Persistence.Queries.Chapters;
using HomeAssignment.Persistence.Queries.Courses;
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

    public async Task<RespondCourseDto> CreateCourseAsync(Guid userId, RequestCreateCourseDto createCourseDto, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started creating course for UserId: {UserId}. Course: {@CourseDto}", userId, createCourseDto);

        var course = mapper.Map<Course>(createCourseDto);
        course.UserId = userId;

        course = await ExecuteTransactionAsync(
            async () => await mediator.Send(new CreateCourseCommand(course), cancellationToken),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Course created successfully. CourseId: {CourseId}", course.Id);
        return mapper.Map<RespondCourseDto>(course);
    }

    public async Task<RespondCourseDto> UpdateCourseAsync(Guid userId, Guid courseId, RequestPartialCourseDto courseDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started updating course. CourseId: {CourseId}, UserId: {UserId}", courseId, userId);
        
        var course = await mediator.Send(new GetCourseByOwnerIdQuery(courseId, userId), cancellationToken);
        if (course == null)
        {
            _logger.LogWarning("Course with ID: {CourseId} not found.", courseId);
            throw new ArgumentException("Course does not exist.");
        }

        course.PatchUpdate(courseDto.Title, courseDto.Description, courseDto.ImageUrl, courseDto.CategoryId ?? course.CategoryId);

        var updatedCourse = await ExecuteTransactionAsync(
            async () => await mediator.Send(new UpdateCourseCommand(courseId, course), cancellationToken),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Course updated successfully. CourseId: {CourseId}", updatedCourse.Id);
        return mapper.Map<RespondCourseDto>(updatedCourse);
    }

    public async Task DeleteCourseAsync(Guid userId, Guid courseId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started deleting course. CourseId: {CourseId}, UserId: {UserId}", courseId, userId);

        await ExecuteTransactionAsync(
            async () => await mediator.Send(new DeleteCourseCommand(courseId), cancellationToken),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Course deleted successfully. CourseId: {CourseId}", courseId);
    }

    public async Task<RespondCourseDto> PublishCourseAsync(Guid userId, Guid courseId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started publishing course. CourseId: {CourseId}, UserId: {UserId}", courseId, userId);

        var hasPublishedChapters = await mediator.Send(new IsAnyPublishedChapterByCourseIdQuery(courseId), cancellationToken);
        if (!hasPublishedChapters)
        {
            _logger.LogInformation("Course with ID: {CourseId} cannot be published. No published chapters found.", courseId);
            throw new ArgumentException("Course does not have published chapters.");
        }

        var course = await mediator.Send(new GetCourseByOwnerIdQuery(courseId, userId), cancellationToken);
        if (course == null)
        {
            _logger.LogWarning("Course with ID: {CourseId} not found.", courseId);
            throw new ArgumentException("Course does not exist.");
        }

        course.MarkAsPublished();

        var updatedCourse = await ExecuteTransactionAsync(
            async () => await mediator.Send(new UpdateCourseCommand(courseId, course), cancellationToken),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Course published successfully. CourseId: {CourseId}", updatedCourse.Id);
        return mapper.Map<RespondCourseDto>(updatedCourse);
    }

    public async Task<RespondCourseDto> UnpublishCourseAsync(Guid userId, Guid courseId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started unpublishing course. CourseId: {CourseId}, UserId: {UserId}", courseId, userId);

        var course = await mediator.Send(new GetCourseByOwnerIdQuery(courseId, userId), cancellationToken);
        if (course == null)
        {
            _logger.LogWarning("Course with ID: {CourseId} not found.", courseId);
            throw new ArgumentException("Course does not exist.");
        }

        course.MarkAsUnpublished();

        var updatedCourse = await ExecuteTransactionAsync(
            async () => await mediator.Send(new UpdateCourseCommand(courseId, course), cancellationToken),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Course unpublished successfully. CourseId: {CourseId}", updatedCourse.Id);
        return mapper.Map<RespondCourseDto>(updatedCourse);
    }
    
    public async Task<PagedList<RespondCourseFullInfoDto>> GetCoursesFullInfoAsync(RequestCourseFilterParameters filterParameters, Guid userId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving all courses for UserId: {UserId} with filter: {@Filter}", userId, filterParameters);
        
        var query = new GetAllCourseDetailViewsQuery(filterParameters, userId);
        var result = await mediator.Send(query, cancellationToken);

        var mappedItems = new List<RespondCourseFullInfoDto>();
        
        if (filterParameters.IncludeProgress)
        {
            foreach (var course in result.Items)
            {
                var progress = await mediator.Send(new GetUserProgressPercentageQuery(userId, course.Id), cancellationToken);
                
                var dto = mapper.Map<RespondCourseFullInfoDto>(course);
                dto.Progress = progress;
                mappedItems.Add(dto);
            }
        }
        else
        {
            mappedItems = result.Items
                .Select(mapper.Map<RespondCourseFullInfoDto>)
                .ToList();
        }

        _logger.LogInformation("Retrieved {Count} courses for UserId: {UserId}", mappedItems.Count, userId);
        
        return new PagedList<RespondCourseFullInfoDto>(mappedItems, result.TotalCount, result.Page, result.PageSize);
    }
    
    public async Task<PagedList<RespondCourseFullInfoDto>> GetUserCoursesFullInfoAsync(RequestCourseFilterParameters filterParameters, Guid userId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving all courses for UserId: {UserId}", userId);
        
        var query = new GetAllCourseDetailViewsByOwnerIdQuery(filterParameters, userId);
        var result = await mediator.Send(query, cancellationToken);

        var mappedItems = result.Items
            .Select(mapper.Map<RespondCourseFullInfoDto>)
            .ToList();

        _logger.LogInformation("Retrieved {Count} courses for UserId: {UserId}", mappedItems.Count, userId);
        
        return new PagedList<RespondCourseFullInfoDto>(mappedItems, result.TotalCount, result.Page, result.PageSize);
    }
    
    public async Task<RespondCourseFullInfoDto?> GetSingleCourseFullInfoAsync(RequestCourseFilterParameters filterParameters, Guid userId, Guid courseId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving course for CourseId: {CourseId}", courseId);
        
        var query = new GetSingleCourseDetailViewByOwnerIdQuery(filterParameters, userId, courseId);
        var result = await mediator.Send(query, cancellationToken);

        if (query.FilterParameters.IncludeChapters && result?.Chapters != null)
        {
            result.Chapters = result.Chapters
                .OrderBy(ch => ch.Position)
                .ToList();
        }
        
        _logger.LogInformation("Retrieved course for CourseId: {CourseId}", courseId);
        
        return mapper.Map<RespondCourseFullInfoDto>(result);
    }
}