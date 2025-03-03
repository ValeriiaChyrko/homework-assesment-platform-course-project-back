using AutoMapper;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.Domain.Abstractions.Enums;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.DTOs.SharedDTOs;
using HomeAssignment.Persistence.Commands.Chapters;
using HomeAssignment.Persistence.Commands.UserProgresses;
using HomeAssignment.Persistence.Queries.Assignments;
using HomeAssignment.Persistence.Queries.Chapters;
using HomeAssignment.Persistence.Queries.Courses;
using HomeAssignment.Persistence.Queries.UserProgresses;
using HomeAssignment.Persistence.Queries.Users;
using HomeworkAssignment.Application.Abstractions;
using HomeworkAssignment.Application.Abstractions.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HomeworkAssignment.Application.Implementations;

public class ChapterService : BaseService<ChapterService>, IChapterService
{
    private readonly ICourseService _courseService;
    
    private readonly ILogger<ChapterService> _logger;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ChapterService(ICourseService courseService, IDatabaseTransactionManager transactionManager, 
        ILogger<ChapterService> logger, IMediator mediator, IMapper mapper)
        : base(logger, transactionManager)
    {
        _courseService = courseService;
        _logger = logger;
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<RespondChapterDto> CreateChapterAsync(Guid userId, Guid courseId, RequestChapterDto chapterDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started creating chapter: {@ChapterDto}", chapterDto);
        var isTeacher = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new CheckIfUserInRoleQuery(userId, UserRoles.Teacher), cancellationToken),
            cancellationToken: cancellationToken);
        if (!isTeacher)
        {
            _logger.LogWarning("User with ID: {userId} does not have an teacher role.", userId);
            throw new ArgumentException("User does not have an teacher role.");
        }
        
        var lastChapter = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new GetLastChapterByIdQuery(courseId), cancellationToken),
            cancellationToken: cancellationToken);
        var newPosition = lastChapter != null ? lastChapter.Position + 1 : 1;
        
        var chapter = _mapper.Map<Chapter>(chapterDto);
        chapter.Position = newPosition;
        chapter.CourseId = courseId;
        
        chapter = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new CreateChapterCommand(chapter), cancellationToken),
            cancellationToken: cancellationToken);
        
        _logger.LogInformation("Successfully created chapter with ID: {ChapterId}", chapter.Id);
        return _mapper.Map<RespondChapterDto>(chapter);
    }

    public async Task<RespondChapterDto> UpdateChapterAsync(Guid userId, Guid chapterId, Guid courseId, RequestChapterDto chapterDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started updating chapter with ID: {ChapterId}", chapterId);
        var isTeacher = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new CheckIfUserInRoleQuery(userId, UserRoles.Teacher), cancellationToken),
            cancellationToken: cancellationToken);
        if (!isTeacher)
        {
            _logger.LogWarning("User with ID: {userId} does not have an teacher role.", userId);
            throw new ArgumentException("User does not have an teacher role.");
        }
        
        var chapter = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new GetChapterByIdQuery(chapterId, courseId), cancellationToken),
            cancellationToken: cancellationToken);
        if (chapter == null)
        {
            _logger.LogWarning("Chapter with ID: {id} does not exist.", chapterId);
            throw new ArgumentException("Chapter does not exist.");
        }
        chapter.Update(
            chapterDto.Title,
            chapterDto.Description,
            chapterDto.VideoUrl,
            chapterDto.Position,
            chapterDto.IsFree,
            chapterDto.MuxDataId,
            chapterDto.CourseId
        );
        
        var updatedChapter = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new UpdateChapterCommand(chapterId, chapter), cancellationToken),
            cancellationToken: cancellationToken);
        _logger.LogInformation("Successfully updated chapter: {@Chapter}", updatedChapter);
        return _mapper.Map<RespondChapterDto>(updatedChapter);
    }

    public async Task DeleteChapterAsync(Guid userId, Guid courseId, Guid chapterId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started deleting chapter with ID: {ChapterId}", chapterId);
        var isTeacher = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new CheckIfUserInRoleQuery(userId, UserRoles.Teacher), cancellationToken),
            cancellationToken: cancellationToken);
        if (!isTeacher)
        {
            _logger.LogWarning("User with ID: {userId} does not have an teacher role.", userId);
            throw new ArgumentException("User does not have an teacher role.");
        }
        
        await ExecuteTransactionAsync(
            async () => await _mediator.Send(new DeleteChapterCommand(chapterId), cancellationToken),
            cancellationToken: cancellationToken);
        
        var isAnyPublishedChapterInCourse = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new IsAnyPublishedChapterByCourseIdQuery(chapterId), cancellationToken),
            cancellationToken: cancellationToken);
        if (!isAnyPublishedChapterInCourse)
        {
            await _courseService.UnpublishCourseAsync(userId, courseId, cancellationToken);
        }
        
        _logger.LogInformation("Successfully deleted chapter with ID: {ChapterId}", chapterId);
    }

    public async Task<RespondUserProgressDto> UpdateProgressAsync(Guid userId, Guid courseId, Guid chapterId, 
        RequestUserProgressDto userProgressDto, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started updating user progress with CHAPTER_ID: {ChapterId}", chapterId);
        
        var userProgress = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new GetUserProgressByIdQuery(userId, chapterId), cancellationToken),
            cancellationToken: cancellationToken);
        if (userProgress == null)
        {
            _logger.LogInformation("Started creating user progress with CHAPTER_ID: {ChapterId}", chapterId);
            
            var progress = _mapper.Map<UserProgress>(userProgressDto);
            var createdUserProgress = await ExecuteTransactionAsync(
                async () => await _mediator.Send(new CreateUserProgressCommand(progress), cancellationToken),
                cancellationToken: cancellationToken);
            
            _logger.LogInformation("Successfully created user progress with CHAPTER_ID: {ChapterId}", chapterId);
            return _mapper.Map<RespondUserProgressDto>(createdUserProgress);
        }

        var mappedProgress = _mapper.Map<UserProgress>(userProgress);
        mappedProgress.Update(userProgressDto.IsCompleted);
        var updatedUserProgress = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new UpdateUserProgressCommand(mappedProgress), cancellationToken),
            cancellationToken: cancellationToken);
            
        _logger.LogInformation("Successfully updated user progress with CHAPTER_ID: {ChapterId}", chapterId);
        return _mapper.Map<RespondUserProgressDto>(updatedUserProgress);
    }

    public async Task<RespondChapterDto> PublishChapterAsync(Guid userId, Guid courseId, Guid chapterId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started publish chapter with ID: {ChapterId}", chapterId);
        
        var isTeacher = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new CheckIfUserInRoleQuery(userId, UserRoles.Teacher), cancellationToken),
            cancellationToken: cancellationToken);
        if (!isTeacher)
        {
            _logger.LogWarning("User with ID: {userId} does not have an teacher role.", userId);
            throw new ArgumentException("User does not have an teacher role.");
        }
        
        var isAnyPublishedAssignmentInCourse = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new IsAnyPublishedAssignmentByChapterIdQuery(chapterId), cancellationToken),
            cancellationToken: cancellationToken);
        if (!isAnyPublishedAssignmentInCourse)
        {
            _logger.LogInformation("Unable published chapter with ID: {ChapterId}. Chapter does not have published assignments.", chapterId);
            throw new ArgumentException("Chapter does not have published assignments.");
        }
        
        var chapter = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new GetChapterByIdQuery(chapterId, courseId), cancellationToken),
            cancellationToken: cancellationToken);
        if (chapter == null)
        {
            _logger.LogWarning("Chapter with ID: {id} does not exist.", chapterId);
            throw new ArgumentException("Chapter does not exist.");
        }
        chapter.Publish();
        
        var updatedChapter = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new UpdateChapterCommand(chapterId, chapter), cancellationToken),
            cancellationToken: cancellationToken);
        
        _logger.LogInformation("Successfully published chapter: {@Chapter}", updatedChapter);
        return _mapper.Map<RespondChapterDto>(updatedChapter);
    }

    public async Task<RespondChapterDto> UnpublishChapterAsync(Guid userId, Guid courseId, Guid chapterId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started unpublish chapter with ID: {ChapterId}", chapterId);
        
        var isTeacher = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new CheckIfUserInRoleQuery(userId, UserRoles.Teacher), cancellationToken),
            cancellationToken: cancellationToken);
        if (!isTeacher)
        {
            _logger.LogWarning("User with ID: {userId} does not have an teacher role.", userId);
            throw new ArgumentException("User does not have an teacher role.");
        }
        
        var chapter = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new GetChapterByIdQuery(chapterId, courseId), cancellationToken),
            cancellationToken: cancellationToken);
        if (chapter == null)
        {
            _logger.LogWarning("Chapter with ID: {id} does not exist.", chapterId);
            throw new ArgumentException("Chapter does not exist.");
        }
        chapter.Unpublish();
        
        var updatedChapter = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new UpdateChapterCommand(chapterId, chapter), cancellationToken),
            cancellationToken: cancellationToken);
        
        var isAnyPublishedChapterInCourse = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new IsAnyPublishedChapterByCourseIdQuery(courseId), cancellationToken),
            cancellationToken: cancellationToken);
        if (!isAnyPublishedChapterInCourse)
        {
            await _courseService.UnpublishCourseAsync(userId, courseId, cancellationToken);
        }
        
        _logger.LogInformation("Successfully unpublished chapter: {@Chapter}", updatedChapter);
        return _mapper.Map<RespondChapterDto>(updatedChapter);
    }
    
    public async Task ReorderChapterAsync(Guid userId, Guid courseId, IEnumerable<RespondChapterDto> chapterDtos,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started reorder chapters with COURSE_ID: {CourseId}", courseId);
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
        
        await ExecuteTransactionAsync(
            async () =>
            {
                foreach (var chapter in chapterDtos)
                {
                    await _mediator.Send(new UpdatePartialChapterCommand(chapter.Id, chapter.Position), cancellationToken);
                }
            },
            cancellationToken: cancellationToken);
        
        _logger.LogInformation("Successfully reorder chapters with COURSE_ID: {CourseId}", courseId);
    }

    public async Task<PagedList<RespondChapterDto>> GetChaptersAsync(RequestChapterFilterParameters filterParameters, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started retrieving all chapters");

        var query = new GetAllChaptersQuery(filterParameters);
        var result = await ExecuteWithExceptionHandlingAsync(
            async () => await _mediator.Send(query, cancellationToken)
        );
        
        var mappedItems = result.Items.Select(entityModel => _mapper.Map<RespondChapterDto>(entityModel)).ToList();
        var pagedResult = new PagedList<RespondChapterDto>(mappedItems, result.TotalCount, result.Page, result.PageSize);

        _logger.LogInformation("Successfully retrieved all chapters");
        return pagedResult;
    }

    public async Task<RespondChapterDto?> GetChapterByIdAsync(Guid chapterId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started retrieving chapter");
        
        var result = await ExecuteWithExceptionHandlingAsync(
            async () => await _mediator.Send(GetChapterByIdAsync(chapterId), cancellationToken)
        );

        if (result == null)
        {
            _logger.LogWarning("Chapter with ID: {id} does not exist.", chapterId);
            return null;
        }

        _logger.LogInformation("Successfully retrieved chapter");
        return _mapper.Map<RespondChapterDto>(result);
    }
}