using AutoMapper;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.Persistence.Commands.Chapters;
using HomeAssignment.Persistence.Queries.Assignments;
using HomeAssignment.Persistence.Queries.Attachments;
using HomeAssignment.Persistence.Queries.Chapters;
using HomeAssignment.Persistence.Queries.Courses;
using HomeworkAssignment.Application.Abstractions;
using HomeworkAssignment.Application.Abstractions.ChapterRelated;
using HomeworkAssignment.Application.Abstractions.Contracts;
using HomeworkAssignment.Application.Abstractions.CourseRelated;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HomeworkAssignment.Application.Implementations.ChapterRelated
{
    public class ChapterService(
        ICourseService courseService,
        IDatabaseTransactionManager transactionManager,
        ILogger<ChapterService> logger,
        IMediator mediator,
        IMapper mapper)
        : BaseService<ChapterService>(logger, transactionManager), IChapterService
    {
        private readonly ILogger<ChapterService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<RespondChapterDto> CreateChapterAsync(Guid courseId, RequestCreateChapterDto createChapterDto, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Started creating chapter: {@ChapterDto}", createChapterDto);

            var lastChapter = await mediator.Send(new GetLastChapterByIdQuery(courseId), cancellationToken);
            var newPosition = lastChapter != null ? lastChapter.Position + 1 : 0;

            var chapter = mapper.Map<Chapter>(createChapterDto);
            chapter.Position = newPosition;
            chapter.CourseId = courseId;

            chapter = await ExecuteTransactionAsync(
                async () => await mediator.Send(new CreateChapterCommand(chapter), cancellationToken),
            cancellationToken: cancellationToken);
            
            _logger.LogInformation("Successfully created chapter with ID: {ChapterId}", chapter.Id);
            return mapper.Map<RespondChapterDto>(chapter);
        }

        public async Task<RespondChapterDto> UpdateChapterAsync(Guid courseId, Guid chapterId, RequestPartialChapterDto chapterDto, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Started updating chapter with ID: {ChapterId}", chapterId);

            var chapter = await mediator.Send(new GetChapterByIdQuery(courseId, chapterId), cancellationToken);
            if (chapter == null)
            {
                _logger.LogWarning("Chapter with ID: {id} does not exist.", chapterId);
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

            _logger.LogInformation("Successfully updated chapter: {@Chapter}", updatedChapter);
            return mapper.Map<RespondChapterDto>(updatedChapter);
        }

        public async Task DeleteChapterAsync(Guid userId, Guid courseId, Guid chapterId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Started deleting chapter with ID: {ChapterId}", chapterId);
            
           await ExecuteTransactionAsync(
                async () => await mediator.Send(new DeleteChapterCommand(chapterId), cancellationToken),
                cancellationToken: cancellationToken);

            var isAnyPublishedChapterInCourse = await mediator.Send(new IsAnyPublishedChapterByCourseIdQuery(chapterId), cancellationToken);
            if (!isAnyPublishedChapterInCourse)
            {
                await courseService.UnpublishCourseAsync(userId, courseId, cancellationToken);
            }

            _logger.LogInformation("Successfully deleted chapter with ID: {ChapterId}", chapterId);
        }

        public async Task<RespondChapterDto> PublishChapterAsync(Guid courseId, Guid chapterId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Started publishing chapter with ID: {ChapterId}", chapterId);

            var isAnyPublishedAssignmentInCourse = await mediator.Send(new IsAnyPublishedAssignmentByChapterIdQuery(chapterId), cancellationToken);
            if (!isAnyPublishedAssignmentInCourse)
            {
                _logger.LogInformation("Unable to publish chapter with ID: {ChapterId}. Chapter does not have published assignments.", chapterId);
                throw new ArgumentException("Chapter does not have published assignments.");
            }

            var chapter = await mediator.Send(new GetChapterByIdQuery(courseId, chapterId), cancellationToken);
            if (chapter == null)
            {
                _logger.LogWarning("Chapter with ID: {id} does not exist.", chapterId);
                throw new ArgumentException("Chapter does not exist.");
            }

            chapter.MarkAsPublished();

            var updatedChapter = await ExecuteTransactionAsync(
                async () => await mediator.Send(new UpdateChapterCommand(chapterId, chapter), cancellationToken),
                cancellationToken: cancellationToken);
            
            _logger.LogInformation("Successfully published chapter: {@Chapter}", updatedChapter);
            return mapper.Map<RespondChapterDto>(updatedChapter);
        }

        public async Task<RespondChapterDto> UnpublishChapterAsync(Guid userId, Guid courseId, Guid chapterId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Started unpublishing chapter with ID: {ChapterId}", chapterId);

            var chapter = await mediator.Send(new GetChapterByIdQuery(courseId, chapterId), cancellationToken);
            if (chapter == null)
            {
                _logger.LogWarning("Chapter with ID: {id} does not exist.", chapterId);
                throw new ArgumentException("Chapter does not exist.");
            }

            chapter.MarkAsUnpublished();

            var updatedChapter = await ExecuteTransactionAsync(
                async () => await mediator.Send(new UpdateChapterCommand(chapterId, chapter), cancellationToken),
                cancellationToken: cancellationToken);

            var isAnyPublishedChapterInCourse = await mediator.Send(new IsAnyPublishedChapterByCourseIdQuery(courseId), cancellationToken);
            if (!isAnyPublishedChapterInCourse)
            {
                await courseService.UnpublishCourseAsync(userId, courseId, cancellationToken);
            }

            _logger.LogInformation("Successfully unpublished chapter: {@Chapter}", updatedChapter);

            return mapper.Map<RespondChapterDto>(updatedChapter);
        }

        public async Task ReorderChapterAsync(Guid userId, Guid courseId, IEnumerable<RequestReorderChapterDto> chapterDtos, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Started reordering chapters for COURSE_ID: {CourseId}", courseId);
            
            await ExecuteTransactionAsync(
                async () =>
                {
                    foreach (var chapter in chapterDtos)
                    {
                        await mediator.Send(new UpdatePartialChapterCommand(chapter.Id, chapter.Position), cancellationToken);
                    }
                },
                cancellationToken: cancellationToken);

            _logger.LogInformation("Successfully reordered chapters for COURSE_ID: {CourseId}", courseId);
        }

        public async Task<RespondChapterWithAssignmentsDto?> GetChapterByIdAsync(Guid courseId, Guid chapterId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Started retrieving chapter");

            var chapter = await mediator.Send(new GetChapterByIdQuery(courseId, chapterId), cancellationToken);
            if (chapter == null)
            {
                _logger.LogWarning("Chapter with ID: {id} does not exist.", chapterId);
                return null;
            }

            var assignments = await mediator.Send(new GetAllAssignmentsByChapterIdQuery(chapterId), cancellationToken);
            var attachments = await mediator.Send(new GetAllAttachmentsByChapterIdQuery(chapterId), cancellationToken);

            _logger.LogInformation("Successfully retrieved chapter");

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

        public async Task<RespondChapterDto?> GetFirstChapterByCourseIdAsync(Guid courseId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Started retrieving first chapter");

            var chapter = await mediator.Send(new GetFirstChapterByIdQuery(courseId), cancellationToken);
            if (chapter == null)
            {
                _logger.LogWarning("Chapter with COURSE_ID: {courseId} does not exist.", courseId);
                return null;
            }

            _logger.LogInformation("Successfully retrieved first chapter");

            return mapper.Map<RespondChapterDto>(chapter);
        }
    }
}