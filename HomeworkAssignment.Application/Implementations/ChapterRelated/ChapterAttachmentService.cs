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
        _logger.LogInformation("Retrieving attachments for chapter. Chapter ID: {ChapterId}", chapterId);

        var attachments = await ExecuteTransactionAsync(
            async () => await mediator.Send(new GetAllAttachmentsByChapterIdQuery(chapterId), cancellationToken),
            cancellationToken: cancellationToken);
        var attachmentList = attachments.ToList();

        _logger.LogInformation("Retrieved {Count} attachments for chapter. Chapter ID: {ChapterId}",
            attachmentList.Count, chapterId);

        return attachmentList.Select(mapper.Map<RespondAttachmentDto>).ToList();
    }

    public async Task<RespondAttachmentDto> CreateChapterAttachmentAsync(Guid courseId, Guid chapterId,
        RequestAttachmentDto attachmentDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating attachment for chapter. Chapter ID: {ChapterId}, Data: {@AttachmentDto}",
            chapterId, attachmentDto);

        var attachment =
            Attachment.CreateForChapter(chapterId, attachmentDto.Key, attachmentDto.Name, attachmentDto.Url);

        var addedAttachment = await ExecuteTransactionAsync(
            async () => await mediator.Send(new CreateAttachmentCommand(attachment), cancellationToken),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Attachment created. Chapter ID: {ChapterId}, Attachment ID: {AttachmentId}",
            chapterId, addedAttachment.Id);

        return mapper.Map<RespondAttachmentDto>(addedAttachment);
    }

    public async Task DeleteChapterAttachmentAsync(Guid attachmentId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting chapter attachment. Attachment ID: {AttachmentId}", attachmentId);

        await ExecuteTransactionAsync(
            async () => await mediator.Send(new DeleteAttachmentCommand(attachmentId), cancellationToken),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Attachment deleted. Attachment ID: {AttachmentId}", attachmentId);
    }
}