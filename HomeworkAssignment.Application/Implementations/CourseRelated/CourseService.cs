using AutoMapper;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.Domain.Abstractions.Enums;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RequestDTOs.CourseRelated;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.DTOs.SharedDTOs;
using HomeAssignment.Persistence.Commands.Attachments;
using HomeAssignment.Persistence.Commands.ChapterUserProgresses;
using HomeAssignment.Persistence.Commands.Courses;
using HomeAssignment.Persistence.Commands.Enrollments;
using HomeAssignment.Persistence.Queries.Attachments;
using HomeAssignment.Persistence.Queries.Chapters;
using HomeAssignment.Persistence.Queries.Courses;
using HomeAssignment.Persistence.Queries.Enrollments;
using HomeAssignment.Persistence.Queries.UserChapterProgresses;
using HomeAssignment.Persistence.Queries.Users;
using HomeworkAssignment.Application.Abstractions;
using HomeworkAssignment.Application.Abstractions.CategoryRelated;
using HomeworkAssignment.Application.Abstractions.Contracts;
using HomeworkAssignment.Application.Abstractions.CourseRelated;
using MediatR;
using Microsoft.Extensions.Logging;
using Guid = System.Guid;

namespace HomeworkAssignment.Application.Implementations.CourseRelated;

public class CourseService : BaseService<CourseService>, ICourseService
{
    private readonly ICategoryService _categoryService;
    
    private readonly ILogger<CourseService> _logger;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CourseService(IDatabaseTransactionManager transactionManager, IMediator mediator,
        ILogger<CourseService> logger, IMapper mapper, ICategoryService categoryService)
        : base(logger, transactionManager)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper;
        _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
    }

    public async Task<RespondCourseDto> CreateCourseAsync(Guid userId, RequestCourseDto courseDto, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started creating course: {@CourseDto}", courseDto);
        
        var course = _mapper.Map<Course>(courseDto);
        course.UserId = userId;
        
        course = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new CreateCourseCommand(course), cancellationToken),
            cancellationToken: cancellationToken);
        
        _logger.LogInformation("Successfully created course with ID: {CourseId}", course.Id);
        return _mapper.Map<RespondCourseDto>(course);
    }

    public async Task<RespondCourseDto> UpdateCourseAsync(Guid userId, Guid courseId, RequestPartialCourseDto courseDto,
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
            async () => await _mediator.Send(new GetCourseByOwnerIdQuery(courseId, userId), cancellationToken),
            cancellationToken: cancellationToken);
        if (course == null)
        {
            _logger.LogWarning("Course with ID: {id} does not exist.", courseId);
            throw new ArgumentException("Course does not exist.");
        }
        course.Update(
            string.IsNullOrEmpty(courseDto.Title) ? course.Title : courseDto.Title,
            string.IsNullOrEmpty(courseDto.Description) ? course.Description : courseDto.Description,
            string.IsNullOrEmpty(courseDto.ImageUrl) ? course.ImageUrl : courseDto.ImageUrl,
            courseDto.UserId,
            courseDto.CategoryId ?? course.CategoryId
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
            async () => await _mediator.Send(new GetCourseByOwnerIdQuery(courseId, userId), cancellationToken),
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
            async () => await _mediator.Send(new GetCourseByOwnerIdQuery(courseId, userId), cancellationToken),
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

    public async Task<PagedList<RespondCourseDto>> GetCoursesAsync(RequestCourseFilterParameters filterParameters, Guid userId, CancellationToken cancellationToken = default)
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
    
    public async Task<PagedList<RespondCourseWithCategoryDto>> GetCoursesWithCategoryAsync(RequestCourseFilterParameters filterParameters, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started retrieving all courses");
        
        var query = new GetAllCoursesQuery(filterParameters);
        var courses = await ExecuteWithExceptionHandlingAsync(
            () => _mediator.Send(query, cancellationToken)
        );
        var categories = await _categoryService.GetCategoriesAsync(cancellationToken);
        var categoriesDict = categories.ToDictionary(c => c.Id);
        
        var coursesWithCategory = courses.Items.Select(course => new RespondCourseWithCategoryDto
        {
            Id = course.Id,
            Title = course.Title,
            Description = course.Description,
            ImageUrl = course.ImageUrl,
            IsPublished = course.IsPublished,
            UserId = course.UserId,
            Category = course.CategoryId != null && categoriesDict.TryGetValue((Guid)course.CategoryId, out var category)
                ? new RespondCategoryDto
                {
                    Id = category.Id,
                    Name = category.Name
                }
                : null
        }).ToList();
        
        var pagedResult = new PagedList<RespondCourseWithCategoryDto>(coursesWithCategory, courses.TotalCount, courses.Page, courses.PageSize);

        _logger.LogInformation("Successfully retrieved all courses");
        return pagedResult;
    }
    
    public async Task<RespondCourseWithCategoryWithProgressDto?> GetCourseWithCategoryWithProgressAsync(Guid courseId, Guid userId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started retrieving course");
        
        var course = await ExecuteWithExceptionHandlingAsync(
            () => _mediator.Send(new GetCourseByIdQuery(courseId), cancellationToken)
        );
        
        var category = await _categoryService.GetCategoryByCourseIdAsync(courseId, cancellationToken);

        var chapters = await ExecuteWithExceptionHandlingAsync(
            () => _mediator.Send(new GetAllPublishedChaptersByCourseIdQuery(course.Id), cancellationToken)
        );

        var chapterIds = chapters.Select(chapter => chapter.Id).ToList();
        
        var totalChapters = chapterIds.Count;
        var completedChapters= await ExecuteWithExceptionHandlingAsync(
            () => _mediator.Send(new GetUserProgressCountQuery(userId, chapterIds), cancellationToken)
        );
        var chaptersWithProgress = new List<RespondChapterWithUserProgressDto>();
        foreach (var chapter in chapters)
        {
            var userProgress = await ExecuteWithExceptionHandlingAsync(
                () => _mediator.Send(new GetUserProgressByIdQuery(userId, chapter.Id), cancellationToken)
            );
            chaptersWithProgress.Add(new RespondChapterWithUserProgressDto
            {
                Id = chapter.Id,
                Title = chapter.Title,
                Description = chapter.Description,
                CourseId = chapter.CourseId,
                IsFree = chapter.IsFree,
                IsPublished = chapter.IsPublished,
                MuxDataId = Guid.Empty,
                Position = chapter.Position,
                UserProgress = _mapper.Map<RespondChapterUserProgressDto>(userProgress)
            });
        };
        
        var progressPercentage = totalChapters > 0 
            ? (int)Math.Round((double)completedChapters / totalChapters * 100) 
            : 0;
        
        var enrollment = await ExecuteWithExceptionHandlingAsync(
            () => _mediator.Send(new GetEnrollmentByIdQuery(userId, courseId), cancellationToken)
        );

        _logger.LogInformation("Successfully retrieved course");
        return new RespondCourseWithCategoryWithProgressDto
        {
            Id = course.Id,
            Title = course.Title,
            Description = course.Description,
            ImageUrl = course.ImageUrl,
            IsPublished = course.IsPublished,
            UserId = course.UserId,
            Category = category != null 
                ? new RespondCategoryDto
                {
                    Id = category.Id,
                    Name = category.Name
                }
                : null,
            Chapters = chaptersWithProgress,
            Progress = progressPercentage,
            IsEnrolled = enrollment != null,
        };
    }
    
    public async Task<RespondCourseWithChapters?> GetCourseWithChaptersByIdAsync(Guid courseId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started retrieving course");
        
        var course = await ExecuteWithExceptionHandlingAsync(
            () => _mediator.Send(new GetCourseByIdQuery(courseId), cancellationToken)
        );

        var chapters = await ExecuteWithExceptionHandlingAsync(
            () => _mediator.Send(new GetAllPublishedChaptersByCourseIdQuery(course.Id), cancellationToken)
        );
        
        _logger.LogInformation("Successfully retrieved course");
        return new RespondCourseWithChapters
        {
            Id = course.Id,
            Title = course.Title,
            Description = course.Description,
            ImageUrl = course.ImageUrl,
            IsPublished = course.IsPublished,
            UserId = course.UserId,
            Chapters = chapters.Select(entityModel => _mapper.Map<RespondChapterDto>(entityModel)).ToList()
        };
    }

    public async Task<PagedList<RespondCourseWithCategoryProgressDto>> GetCoursesWithCategoryWithProgressAsync(RequestCourseFilterParameters filterParameters, Guid userId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started retrieving enrolled courses with {User Id}", userId);
        
        var courses = await ExecuteWithExceptionHandlingAsync(
            () => _mediator.Send(new GetAllCoursesQuery(filterParameters), cancellationToken)
        );

        var categories = await _categoryService.GetCategoriesAsync(cancellationToken);
        var categoriesDict = categories.ToDictionary(c => c.Id);

        var coursesList = courses.Items.ToList();
        var resultList = new List<RespondCourseWithCategoryProgressDto>();

        foreach (var course in coursesList)
        {
            var chapters = await ExecuteWithExceptionHandlingAsync(
                () => _mediator.Send(new GetAllPublishedChaptersByCourseIdQuery(course.Id), cancellationToken)
            );

            var chapterIds = chapters.Select(chapter => chapter.Id).ToList();
            var progress = await ExecuteWithExceptionHandlingAsync(
                () => _mediator.Send(new GetUserProgressCountQuery(userId, chapterIds), cancellationToken)
            );

            resultList.Add(new RespondCourseWithCategoryProgressDto
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                ImageUrl = course.ImageUrl,
                IsPublished = course.IsPublished,
                UserId = course.UserId,
                Category = course.CategoryId != null && categoriesDict.TryGetValue((Guid)course.CategoryId, out var category)
                    ? new RespondCategoryDto
                    {
                        Id = category.Id,
                        Name = category.Name
                    }
                    : null,
                Chapters = chapters.Select(entityModel => _mapper.Map<RespondChapterDto>(entityModel)).ToList(),
                Progress = progress,
            });
        }
        
        _logger.LogInformation("Successfully retrieved all courses with {User Id}", userId);
        return new PagedList<RespondCourseWithCategoryProgressDto>(resultList, courses.TotalCount, courses.Page, courses.PageSize);
    }

    public async Task<PagedList<RespondCourseWithCategoryProgressDto>> GetEnrolledCoursesWithCategoryWithProgressAsync(Guid userId, RequestCourseFilterParameters filterParameters,
    CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started retrieving enrolled courses with {User  Id}", userId);
        
        var enrollments = await ExecuteWithExceptionHandlingAsync(
            () => _mediator.Send(new GetAllEnrollmentsByUserIdQuery(userId), cancellationToken)
        );

        var categories = await _categoryService.GetCategoriesAsync(cancellationToken);
        var categoriesDict = categories.ToDictionary(c => c.Id);

        var enrollmentList = enrollments.ToList();
        var items = new List<RespondCourseWithCategoryProgressDto>();

        foreach (var enrollment in enrollmentList)
        {
            var course = await ExecuteWithExceptionHandlingAsync(
                () => _mediator.Send(new GetCourseByOwnerIdQuery((Guid)enrollment.CourseId!, userId), cancellationToken)
            );

            if (course == null)
            {
                _logger.LogWarning("Course with ID: {id} does not exist.", enrollment.CourseId);
                continue;
            }

            if (!course.IsPublished)
            {
                continue;
            }

            var chapters = await ExecuteWithExceptionHandlingAsync(
                () => _mediator.Send(new GetAllPublishedChaptersByCourseIdQuery(course.Id), cancellationToken)
            );

            var chapterList = chapters.ToList();
            var chapterIds = chapterList.Select(chapter => chapter.Id).ToList();
            var progress = await ExecuteWithExceptionHandlingAsync(
                () => _mediator.Send(new GetUserProgressCountQuery(userId, chapterIds), cancellationToken)
            );

            var courseDto = new RespondCourseWithCategoryProgressDto
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                ImageUrl = course.ImageUrl,
                IsPublished = course.IsPublished,
                UserId = course.UserId,
                Category = course.CategoryId != null && categoriesDict.TryGetValue((Guid)course.CategoryId, out var category)
                    ? new RespondCategoryDto
                    {
                        Id = category.Id,
                        Name = category.Name
                    }
                    : null,
                Chapters = chapterList.Select(entityModel => _mapper.Map<RespondChapterDto>(entityModel)).ToList(),
                Progress = progress,
            };

            items.Add(courseDto);
        }

        var pagedResult = new PagedList<RespondCourseWithCategoryProgressDto>(items, enrollmentList.Count(), filterParameters.PageNumber, filterParameters.PageSize);

        _logger.LogInformation("Successfully retrieved all courses with {User  Id}", userId);
        return pagedResult;
    }

    public async Task<RespondCourseDto?> GetCourseByIdAsync(Guid courseId, Guid userId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started retrieving course");
        
        var result = await ExecuteWithExceptionHandlingAsync(
            async () => await _mediator.Send(new GetCourseByOwnerIdQuery(courseId, userId), cancellationToken)
        );

        if (result == null)
        {
            _logger.LogWarning("Course with ID: {id} does not exist.", courseId);
            return null;
        }

        _logger.LogInformation("Successfully retrieved course");
        return _mapper.Map<RespondCourseDto>(result);
    }
    
    public async Task<RespondCourseWithChaptersWithAttachmentsDto?> GetCourseWithChaptersWithAttachmentsByIdAsync(Guid courseId,  Guid userId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started retrieving course");
        
        var course = await ExecuteWithExceptionHandlingAsync(
            async () => await _mediator.Send(new GetCourseByOwnerIdQuery(courseId, userId), cancellationToken)
        );

        if (course == null)
        {
            _logger.LogWarning("Course with ID: {id} does not exist.", courseId);
            return null;
        }
        
        var chapters = await ExecuteWithExceptionHandlingAsync(
            async () => await _mediator.Send(new GetAllChaptersByCourseIdQuery(courseId), cancellationToken)
        );
        
        var attachments = await ExecuteWithExceptionHandlingAsync(
            async () => await _mediator.Send(new GetAllAttachmentsByCourseIdQuery(courseId), cancellationToken)
        );

        var courseWithChapters = new RespondCourseWithChaptersWithAttachmentsDto
        {
            Id = course.Id,
            Title = course.Title,
            Description = course.Description,
            ImageUrl = course.ImageUrl,
            IsPublished = course.IsPublished,
            UserId = course.UserId,
            CategoryId = course.CategoryId,
            Chapters = chapters.Select(entityModel => _mapper.Map<RespondChapterDto>(entityModel)).ToList(),
            Attachments = attachments.Select(entityModel => _mapper.Map<RespondAttachmentDto>(entityModel)).ToList()
        };

        _logger.LogInformation("Successfully retrieved course");
        return courseWithChapters;
    }
}