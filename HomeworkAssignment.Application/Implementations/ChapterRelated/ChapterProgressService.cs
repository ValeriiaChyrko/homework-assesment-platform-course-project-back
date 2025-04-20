using AutoMapper;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RequestDTOs.ChapterRelated;
using HomeAssignment.DTOs.RespondDTOs.ChapterRelated;
using HomeAssignment.Persistence.Commands.ChapterUserProgresses;
using HomeAssignment.Persistence.Queries.UserChapterProgresses;
using HomeworkAssignment.Application.Abstractions;
using HomeworkAssignment.Application.Abstractions.ChapterRelated;
using HomeworkAssignment.Application.Abstractions.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HomeworkAssignment.Application.Implementations.ChapterRelated;

public class ChapterProgressService(
    ILogger<ChapterProgressService> logger,
    IDatabaseTransactionManager transactionManager,
    IMediator mediator,
    IMapper mapper)
    : BaseService<ChapterProgressService>(logger, transactionManager), IChapterProgressService
{
    private readonly ILogger<ChapterProgressService>
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<RespondChapterUserProgressDto?> GetProgressAsync(Guid userId, Guid courseId, Guid chapterId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started getting user progress with CHAPTER_ID: {ChapterId}", chapterId);

        var userProgress = await ExecuteTransactionAsync(
            async () => await mediator.Send(new GetUserChapterProgressByIdQuery(userId, chapterId), cancellationToken),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Successfully getting user progress with CHAPTER_ID: {ChapterId}", chapterId);
        return userProgress != null ? mapper.Map<RespondChapterUserProgressDto>(userProgress) : null;
    }

    public async Task<RespondChapterUserProgressDto> UpdateProgressAsync(Guid userId, Guid courseId, Guid chapterId,
        RequestChapterUserProgressDto chapterUserProgressDto, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started updating user progress with CHAPTER_ID: {ChapterId}", chapterId);

        var userProgress = await ExecuteTransactionAsync(
            async () => await mediator.Send(new GetUserChapterProgressByIdQuery(userId, chapterId), cancellationToken),
            cancellationToken: cancellationToken);

        if (userProgress == null)
            return await CreateUserProgressAsync(userId, chapterId, chapterUserProgressDto, cancellationToken);

        return await UpdateUserProgressAsync(userProgress, chapterUserProgressDto, cancellationToken);
    }

    private async Task<RespondChapterUserProgressDto> CreateUserProgressAsync(Guid userId, Guid chapterId,
        RequestChapterUserProgressDto chapterUserProgressDto, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Started creating user progress with CHAPTER_ID: {ChapterId}", chapterId);

        var progress = ChapterUserProgress.CreateForChapter(chapterUserProgressDto.IsCompleted, userId, chapterId);
        var createdUserProgress = await ExecuteTransactionAsync(
            async () => await mediator.Send(new CreateUserProgressCommand(progress), cancellationToken),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Successfully created user progress with CHAPTER_ID: {ChapterId}", chapterId);
        return mapper.Map<RespondChapterUserProgressDto>(createdUserProgress);
    }

    private async Task<RespondChapterUserProgressDto> UpdateUserProgressAsync(ChapterUserProgress userProgress,
        RequestChapterUserProgressDto chapterUserProgressDto, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Started updating user progress for CHAPTER_ID: {ChapterId}", userProgress.ChapterId);

        userProgress.UpdateProgress(chapterUserProgressDto.IsCompleted);
        var updatedUserProgress = await ExecuteTransactionAsync(
            async () => await mediator.Send(new UpdateUserProgressCommand(userProgress), cancellationToken),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Successfully updated user progress with CHAPTER_ID: {ChapterId}",
            userProgress.ChapterId);
        return mapper.Map<RespondChapterUserProgressDto>(updatedUserProgress);
    }
}