using AutoMapper;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.Domain.Abstractions.Enums;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.DTOs.SharedDTOs;
using HomeAssignment.Persistence.Commands.Attachments;
using HomeAssignment.Persistence.Commands.Courses;
using HomeAssignment.Persistence.Commands.Enrollments;
using HomeAssignment.Persistence.Queries.Chapters;
using HomeAssignment.Persistence.Queries.Courses;
using HomeAssignment.Persistence.Queries.Enrollments;
using HomeAssignment.Persistence.Queries.Users;
using HomeworkAssignment.Application.Abstractions;
using HomeworkAssignment.Application.Abstractions.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HomeworkAssignment.Application.Implementations;

public class CourseService : BaseService<CourseService>, ICourseService
{
    private readonly ILogger<CourseService> _logger;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CourseService(IDatabaseTransactionManager transactionManager, IMediator mediator,
        ILogger<CourseService> logger, IMapper mapper)
        : base(logger, transactionManager)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper;
    }

    public async Task<RespondCourseDto> CreateCourseAsync(Guid userId, RequestCourseDto courseDto, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started creating course: {@CourseDto}", courseDto);
        var isTeacher = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new CheckIfUserInRoleQuery(userId, UserRoles.Teacher), cancellationToken),
            cancellationToken: cancellationToken);
        if (!isTeacher)
        {
            _logger.LogWarning("User with ID: {userId} does not have an teacher role.", userId);
            throw new ArgumentException("User does not have an teacher role.");
        }
        
        var course = _mapper.Map<Course>(courseDto);
        course.UserId = userId;
        
        course = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new CreateCourseCommand(course), cancellationToken),
            cancellationToken: cancellationToken);
        
        _logger.LogInformation("Successfully created course with ID: {CourseId}", course.Id);
        return _mapper.Map<RespondCourseDto>(course);
    }

    public async Task<RespondCourseDto> UpdateCourseAsync(Guid userId, Guid courseId, RequestCourseDto courseDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started updating course with ID: {CourseId}", courseId);
        var isTeacher = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new CheckIfUserInRoleQuery(userId, UserRoles.Teacher), cancellationToken),
            cancellationToken: cancellationToken);
        if (!isTeacher)
        {
            _logger.LogWarning("User with ID: {userId} does not have an teacher role.", userId);
            throw new ArgumentException("User does not have an teacher role.");
        }
        
        var course = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new GetCourseByIdQuery(courseId, userId), cancellationToken),
            cancellationToken: cancellationToken);
        if (course == null)
        {
            _logger.LogWarning("Course with ID: {id} does not exist.", courseId);
            throw new ArgumentException("Course does not exist.");
        }
        course.Update(
            courseDto.Title,
            courseDto.Description,
            courseDto.ImageUrl,
            courseDto.UserId,
            courseDto.CategoryId
        );
        
        var updatedCourse = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new UpdateCourseCommand(courseId, course), cancellationToken),
            cancellationToken: cancellationToken);
        _logger.LogInformation("Successfully updated course: {@CourseDto}", updatedCourse);
        return _mapper.Map<RespondCourseDto>(updatedCourse);
    }

    public async Task DeleteCourseAsync(Guid userId, Guid courseId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started deleting course with ID: {CourseId}", courseId);
        var isTeacher = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new CheckIfUserInRoleQuery(userId, UserRoles.Teacher), cancellationToken),
            cancellationToken: cancellationToken);
        if (!isTeacher)
        {
            _logger.LogWarning("User with ID: {userId} does not have an teacher role.", userId);
            throw new ArgumentException("User does not have an teacher role.");
        }
        
        //TODO: Check if chapters has video and delete it
        
        await ExecuteTransactionAsync(
            async () => await _mediator.Send(new DeleteCourseCommand(courseId), cancellationToken),
            cancellationToken: cancellationToken);
        
        _logger.LogInformation("Successfully deleted course with ID: {CourseId}", courseId);
    }

    public async Task<RespondCourseDto> PublishCourseAsync(Guid userId, Guid courseId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started publish course with ID: {CourseId}", courseId);
        var isTeacher = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new CheckIfUserInRoleQuery(userId, UserRoles.Teacher), cancellationToken),
            cancellationToken: cancellationToken);
        if (!isTeacher)
        {
            _logger.LogWarning("User with ID: {userId} does not have an teacher role.", userId);
            throw new ArgumentException("User does not have an teacher role.");
        }
        
        var isAnyPublishedChapterInCourse = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new IsAnyPublishedChapterByCourseIdQuery(courseId), cancellationToken),
            cancellationToken: cancellationToken);
        if (!isAnyPublishedChapterInCourse)
        {
            _logger.LogInformation("Unable published course with ID: {CourseId}. Course does not have published chapters.", courseId);
            throw new ArgumentException("Course does not have published chapters.");
        }
        
        var course = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new GetCourseByIdQuery(courseId, userId), cancellationToken),
            cancellationToken: cancellationToken);
        if (course == null)
        {
            _logger.LogWarning("Course with ID: {id} does not exist.", courseId);
            throw new ArgumentException("Course does not exist.");
        }
        course.Publish();
        
        var updatedCourse = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new UpdateCourseCommand(courseId, course), cancellationToken),
            cancellationToken: cancellationToken);
        _logger.LogInformation("Successfully published course: {@CourseDto}", updatedCourse);
        return _mapper.Map<RespondCourseDto>(updatedCourse);
    }

    public async Task<RespondCourseDto> UnpublishCourseAsync(Guid userId, Guid courseId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started unpublish course with ID: {CourseId}", courseId);
        var isTeacher = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new CheckIfUserInRoleQuery(userId, UserRoles.Teacher), cancellationToken),
            cancellationToken: cancellationToken);
        if (!isTeacher)
        {
            _logger.LogWarning("User with ID: {userId} does not have an teacher role.", userId);
            throw new ArgumentException("User does not have an teacher role.");
        }
        
        var course = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new GetCourseByIdQuery(courseId, userId), cancellationToken),
            cancellationToken: cancellationToken);
        if (course == null)
        {
            _logger.LogWarning("Course with ID: {id} does not exist.", courseId);
            throw new ArgumentException("Course does not exist.");
        }
        course.Unpublish();
        
        var updatedCourse = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new UpdateCourseCommand(courseId, course), cancellationToken),
            cancellationToken: cancellationToken);
        
        _logger.LogInformation("Successfully unpublished course: {@CourseDto}", updatedCourse);
        return _mapper.Map<RespondCourseDto>(updatedCourse);
    }

    public async Task<RespondEnrollmentDto> EnrollAsync(Guid userId, Guid courseId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started enrolling course with ID: {CourseId}", courseId);
        
        var course = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new GetCourseByIdQuery(courseId, userId), cancellationToken),
            cancellationToken: cancellationToken);
        if (course == null)
        {
            _logger.LogWarning("Course with ID: {id} does not exist.", courseId);
            throw new ArgumentException("Course does not exist.");
        }
        
        var existingEnrollment = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new GetEnrollmentByIdQuery(userId, courseId), cancellationToken),
            cancellationToken: cancellationToken);
        
        if (existingEnrollment != null)
        {
            _logger.LogWarning("Enrollment with COURSE_ID: {id} already exist.", courseId);
            throw new ArgumentException("Already Enrolled.");
        }
        
        var enrollment = Enrollment.Create(userId, courseId);
        
        var addedEnrollment =  await ExecuteTransactionAsync(
            async () => await _mediator.Send(new CreateEnrollmentCommand(enrollment), cancellationToken),
            cancellationToken: cancellationToken);
        
        _logger.LogInformation("Successfully enrolled course with ID: {CourseId}", courseId);
        return _mapper.Map<RespondEnrollmentDto>(addedEnrollment);
    }

    public async Task<PagedList<RespondCourseDto>> GetCoursesAsync(RequestCourseFilterParameters filterParameters, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started retrieving all courses");

        var query = new GetAllCoursesQuery(filterParameters);
        var result = await ExecuteWithExceptionHandlingAsync(
            async () => await _mediator.Send(query, cancellationToken)
        );
        
        var mappedItems = result.Items.Select(entityModel => _mapper.Map<RespondCourseDto>(entityModel)).ToList();
        var pagedResult = new PagedList<RespondCourseDto>(mappedItems, result.TotalCount, result.Page, result.PageSize);

        _logger.LogInformation("Successfully retrieved all courses");
        return pagedResult;
    }

    public async Task<RespondCourseDto?> GetCourseByIdAsync(Guid courseId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started retrieving course");
        
        var result = await ExecuteWithExceptionHandlingAsync(
            async () => await _mediator.Send(GetCourseByIdAsync(courseId), cancellationToken)
        );

        if (result == null)
        {
            _logger.LogWarning("Course with ID: {id} does not exist.", courseId);
            return null;
        }

        _logger.LogInformation("Successfully retrieved course");
        return _mapper.Map<RespondCourseDto>(result);
    }

    public async Task<RespondAttachmentDto> CreateCourseAttachmentAsync(Guid userId, Guid courseId, RequestAttachmentDto attachmentDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started creating course attachment with COURSE_ID: {CourseId}", courseId);
        
        var isTeacher = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new CheckIfUserInRoleQuery(userId, UserRoles.Teacher), cancellationToken),
            cancellationToken: cancellationToken);
        if (!isTeacher)
        {
            _logger.LogWarning("User with ID: {userId} does not have an teacher role.", userId);
            throw new ArgumentException("User does not have an teacher role.");
        }
        
        var course = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new GetCourseByIdQuery(courseId, userId), cancellationToken),
            cancellationToken: cancellationToken);
        if (course == null)
        {
            _logger.LogWarning("Course with ID: {id} does not exist.", courseId);
            throw new ArgumentException("Course does not exist.");
        }
        
        var attachment = Attachment.CreateForCourse(courseId, attachmentDto.Name, attachmentDto.Url);
        
        var addedAttachment =  await ExecuteTransactionAsync(
            async () => await _mediator.Send(new CreateAttachmentCommand(attachment), cancellationToken),
            cancellationToken: cancellationToken);
        
        _logger.LogInformation("Successfully created course attachment with COURSE_ID: {CourseId}", courseId);
        return _mapper.Map<RespondAttachmentDto>(addedAttachment);
    }

    public async Task DeleteCourseAttachmentAsync(Guid userId, Guid courseId, Guid attachmentId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started deleting course attachment with ID: {AttachmentId}", attachmentId);
        
        var isTeacher = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new CheckIfUserInRoleQuery(userId, UserRoles.Teacher), cancellationToken),
            cancellationToken: cancellationToken);
        if (!isTeacher)
        {
            _logger.LogWarning("User with ID: {userId} does not have an teacher role.", userId);
            throw new ArgumentException("User does not have an teacher role.");
        }
        
        var course = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new GetCourseByIdQuery(courseId, userId), cancellationToken),
            cancellationToken: cancellationToken);
        if (course == null)
        {
            _logger.LogWarning("Course with ID: {id} does not exist.", courseId);
            throw new ArgumentException("Course does not exist.");
        }
        
        //TODO: delete real attachment on the cloud
        
        await ExecuteTransactionAsync(
            async () => await _mediator.Send(new DeleteAttachmentCommand(attachmentId), cancellationToken),
            cancellationToken: cancellationToken);
        
        _logger.LogInformation("Successfully deleted course attachment with ID: {AttachmentId}", attachmentId);
    }
}