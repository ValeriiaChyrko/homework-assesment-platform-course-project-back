using AutoMapper;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RequestDTOs.ChapterRelated;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.DTOs.RespondDTOs.AssignmentRelated;
using HomeAssignment.DTOs.RespondDTOs.AttachmentRelated;
using HomeAssignment.DTOs.RespondDTOs.ChapterRelated;
using HomeAssignment.Persistence.Commands.Chapters;
using HomeAssignment.Persistence.Queries.Assignments;
using HomeAssignment.Persistence.Queries.Attachments;
using HomeAssignment.Persistence.Queries.Chapters;
using HomeworkAssignment.Application.Abstractions;
using HomeworkAssignment.Application.Abstractions.ChapterRelated;
using HomeworkAssignment.Application.Abstractions.Contracts;
using HomeworkAssignment.Application.Abstractions.CourseRelated;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HomeworkAssignment.Application.Implementations.ChapterRelated;

public class ChapterService(
    ICourseService courseService,
    IDatabaseTransactionManager transactionManager,
    ILogger<ChapterService> logger,
    IMediator mediator,
    IMapper mapper)
    : BaseService<ChapterService>(logger, transactionManager), IChapterService
{
    private readonly ILogger<ChapterService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<RespondChapterDto> CreateChapterAsync(Guid courseId, RequestCreateChapterDto createChapterDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Create chapter: {@ChapterDto}", createChapterDto);

        var lastChapter = await mediator.Send(new GetLastChapterByIdQuery(courseId), cancellationToken);
        var newPosition = lastChapter != null ? lastChapter.Position + 1 : 0;

        var chapter = mapper.Map<Chapter>(createChapterDto);
        chapter.Position = (ushort)newPosition;
        chapter.CourseId = courseId;

        chapter = await ExecuteTransactionAsync(
            async () => await mediator.Send(new CreateChapterCommand(chapter), cancellationToken),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Create chapter with ID: {ChapterId}", chapter.Id);
        return mapper.Map<RespondChapterDto>(chapter);
    }

    public async Task<RespondChapterDto> UpdateChapterAsync(Guid courseId, Guid chapterId,
        RequestPartialChapterDto chapterDto, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Update chapter with ID: {ChapterId}", chapterId);

        var chapter = await mediator.Send(new GetChapterByIdQuery(courseId, chapterId), cancellationToken);
        if (chapter == null)
        {
            _logger.LogWarning("Chapter with ID: {id} does not exist", chapterId);
            throw new ArgumentException("Chapter does not exist.");
        }

        chapter.PatchUpdate(
            chapterDto.Title,
            chapterDto.Description,
            chapterDto.VideoUrl,
            chapter.Position,
            chapterDto.IsFree
        );

        var updatedChapter = await ExecuteTransactionAsync(
            async () => await mediator.Send(new UpdateChapterCommand(chapterId, chapter), cancellationToken),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Update chapter: {@Chapter}", updatedChapter);
        return mapper.Map<RespondChapterDto>(updatedChapter);
    }

    public async Task DeleteChapterAsync(Guid userId, Guid courseId, Guid chapterId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Delete chapter with ID: {ChapterId}", chapterId);

        await ExecuteTransactionAsync(
            async () => await mediator.Send(new DeleteChapterCommand(chapterId), cancellationToken),
            cancellationToken: cancellationToken);

        var isAnyPublishedChapterInCourse =
            await mediator.Send(new IsAnyPublishedChapterByCourseIdQuery(chapterId), cancellationToken);
        if (!isAnyPublishedChapterInCourse)
            await courseService.UnpublishCourseAsync(userId, courseId, cancellationToken);

        _logger.LogInformation("Delete chapter with ID: {ChapterId}", chapterId);
    }

    public async Task<RespondChapterDto> PublishChapterAsync(Guid courseId, Guid chapterId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Publish chapter with ID: {ChapterId}", chapterId);

        var isAnyPublishedAssignmentInCourse =
            await mediator.Send(new IsAnyPublishedAssignmentByChapterIdQuery(chapterId), cancellationToken);
        if (!isAnyPublishedAssignmentInCourse)
        {
            _logger.LogInformation("Chapter with ID: {ChapterId} has no published assignments", chapterId);
            throw new ArgumentException("Chapter does not have published assignments.");
        }

        var chapter = await mediator.Send(new GetChapterByIdQuery(courseId, chapterId), cancellationToken);
        if (chapter == null)
        {
            _logger.LogWarning("Chapter with ID: {id} does not exist", chapterId);
            throw new ArgumentException("Chapter does not exist.");
        }

        chapter.MarkAsPublished();

        var updatedChapter = await ExecuteTransactionAsync(
            async () => await mediator.Send(new UpdateChapterCommand(chapterId, chapter), cancellationToken),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Publish chapter: {@Chapter}", updatedChapter);
        return mapper.Map<RespondChapterDto>(updatedChapter);
    }

    public async Task<RespondChapterDto> UnpublishChapterAsync(Guid userId, Guid courseId, Guid chapterId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Unpublish chapter with ID: {ChapterId}", chapterId);

        var chapter = await mediator.Send(new GetChapterByIdQuery(courseId, chapterId), cancellationToken);
        if (chapter == null)
        {
            _logger.LogWarning("Chapter with ID: {id} does not exist", chapterId);
            throw new ArgumentException("Chapter does not exist.");
        }

        chapter.MarkAsUnpublished();

        var updatedChapter = await ExecuteTransactionAsync(
            async () => await mediator.Send(new UpdateChapterCommand(chapterId, chapter), cancellationToken),
            cancellationToken: cancellationToken);

        var isAnyPublishedChapterInCourse =
            await mediator.Send(new IsAnyPublishedChapterByCourseIdQuery(courseId), cancellationToken);
        if (!isAnyPublishedChapterInCourse)
            await courseService.UnpublishCourseAsync(userId, courseId, cancellationToken);

        _logger.LogInformation("Unpublish chapter: {@Chapter}", updatedChapter);
        return mapper.Map<RespondChapterDto>(updatedChapter);
    }

    public async Task ReorderChapterAsync(Guid courseId, IEnumerable<RequestReorderChapterDto> chapterDtos,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Reorder chapters for COURSE_ID: {CourseId}", courseId);

        await ExecuteTransactionAsync(
            async () =>
            {
                foreach (var chapter in chapterDtos)
                    await mediator.Send(new UpdatePartialChapterCommand(chapter.Id, chapter.Position),
                        cancellationToken);
            },
            cancellationToken: cancellationToken);

        _logger.LogInformation("Reorder chapters for COURSE_ID: {CourseId}", courseId);
    }

    public async Task<List<RespondChapterDto>> GetChaptersAsync(Guid courseId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Get all published chapters");

        var chapters = await mediator.Send(new GetAllPublishedChaptersByCourseIdQuery(courseId), cancellationToken);

        _logger.LogInformation("Return all published chapters");
        return chapters.Select(mapper.Map<RespondChapterDto>).ToList();
    }

    public async Task<RespondChapterWithAssignmentsDto?> GetChapterByIdAsync(Guid courseId, Guid chapterId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Get chapter with ID: {ChapterId}", chapterId);

        var chapter = await mediator.Send(new GetChapterByIdQuery(courseId, chapterId), cancellationToken);
        if (chapter == null)
        {
            _logger.LogWarning("Chapter with ID: {id} does not exist", chapterId);
            return null;
        }

        var assignments = await mediator.Send(new GetAllAssignmentsByChapterIdQuery(chapterId), cancellationToken);
        var attachments = await mediator.Send(new GetAllAttachmentsByChapterIdQuery(chapterId), cancellationToken);

        _logger.LogInformation("Return chapter with assignments and attachments");
        return new RespondChapterWithAssignmentsDto
        {
            Id = chapter.Id,
            Title = chapter.Title,
            Description = chapter.Description,
            VideoUrl = chapter.VideoUrl,
            Position = chapter.Position,
            IsPublished = chapter.IsPublished,
            IsFree = chapter.IsFree,
            CourseId = chapter.CourseId,
            Assignments = assignments.Select(mapper.Map<RespondAssignmentDto>).ToList(),
            Attachments = attachments.Select(mapper.Map<RespondAttachmentDto>).ToList()
        };
    }

    public async Task<RespondChapterDto?> GetFirstChapterByCourseIdAsync(Guid courseId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Get first chapter for COURSE_ID: {CourseId}", courseId);

        var chapter = await mediator.Send(new GetFirstChapterByIdQuery(courseId), cancellationToken);

        _logger.LogInformation("Return first chapter for COURSE_ID: {CourseId}", courseId);
        return chapter == null ? null : mapper.Map<RespondChapterDto>(chapter);
    }

    public async Task<RespondChapterDto?> GetNextChapterByChapterIdAsync(Guid courseId, Guid chapterId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Get next chapter after CHAPTER_ID: {ChapterId}", chapterId);

        var chapter = await mediator.Send(new GetChapterByIdQuery(courseId, chapterId), cancellationToken);
        var nextPosition = chapter != null ? chapter.Position + 1 : 0;

        var nextChapter = await mediator.Send(new GetPublishedChapterByPositionQuery((ushort)nextPosition, courseId),
            cancellationToken);

        _logger.LogInformation("Return next chapter after CHAPTER_ID: {ChapterId}", chapterId);
        return nextChapter == null ? null : mapper.Map<RespondChapterDto>(nextChapter);
    }
}