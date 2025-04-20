using AutoMapper;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.Persistence.Commands.Attachments;
using HomeAssignment.Persistence.Queries.Attachments;
using HomeworkAssignment.Application.Abstractions;
using HomeworkAssignment.Application.Abstractions.ChapterRelated;
using HomeworkAssignment.Application.Abstractions.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HomeworkAssignment.Application.Implementations.ChapterRelated;

public class ChapterAttachmentService(
    ILogger<ChapterAttachmentService> logger,
    IDatabaseTransactionManager transactionManager,
    IMediator mediator,
    IMapper mapper)
    : BaseService<ChapterAttachmentService>(logger, transactionManager), IChapterAttachmentService
{
    private readonly ILogger<ChapterAttachmentService> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<IReadOnlyList<RespondAttachmentDto>> GetChapterAttachmentsAsync(Guid courseId, Guid chapterId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started getting chapter attachments with CHAPTER_ID: {ChapterId}", chapterId);

        var attachments = await ExecuteTransactionAsync(
            async () => await mediator.Send(new GetAllAttachmentsByChapterIdQuery(chapterId), cancellationToken),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Successfully got chapter attachments with CHAPTER_ID: {ChapterId}", chapterId);

        return attachments.Select(mapper.Map<RespondAttachmentDto>).ToList();
    }

    public async Task<RespondAttachmentDto> CreateChapterAttachmentAsync(Guid courseId, Guid chapterId,
        RequestAttachmentDto attachmentDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started creating chapter attachment with CHAPTER_ID: {ChapterId}", chapterId);

        var attachment =
            Attachment.CreateForChapter(chapterId, attachmentDto.Key, attachmentDto.Name, attachmentDto.Url);

        var addedAttachment = await ExecuteTransactionAsync(
            async () => await mediator.Send(new CreateAttachmentCommand(attachment), cancellationToken),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Successfully created chapter attachment with CHAPTER_ID: {ChapterId}", chapterId);

        return mapper.Map<RespondAttachmentDto>(addedAttachment);
    }

    public async Task DeleteChapterAttachmentAsync(Guid attachmentId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started deleting chapter attachment with ATTACHMENT_ID: {AttachmentId}", attachmentId);

        await ExecuteTransactionAsync(
            async () => await mediator.Send(new DeleteAttachmentCommand(attachmentId), cancellationToken),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Successfully deleted chapter attachment with ATTACHMENT_ID: {AttachmentId}",
            attachmentId);
    }
}