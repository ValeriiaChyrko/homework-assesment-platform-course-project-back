using AutoMapper;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.Persistence.Commands.Attachments;
using HomeAssignment.Persistence.Queries.Attachments;
using HomeworkAssignment.Application.Abstractions;
using HomeworkAssignment.Application.Abstractions.Contracts;
using HomeworkAssignment.Application.Abstractions.CourseRelated;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HomeworkAssignment.Application.Implementations.CourseRelated;

public class CourseAttachmentService(
    ILogger<CourseAttachmentService> logger,
    IDatabaseTransactionManager transactionManager,
    IMediator mediator,
    IMapper mapper)
    : BaseService<CourseAttachmentService>(logger, transactionManager), ICourseAttachmentService
{
    private readonly ILogger<CourseAttachmentService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<IReadOnlyList<RespondAttachmentDto>> GetCourseAttachmentsAsync(Guid courseId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching attachments for COURSE_ID: {CourseId}", courseId);
        
        var attachments = await ExecuteTransactionAsync(
            () => mediator.Send(new GetAllAttachmentsByCourseIdQuery(courseId), cancellationToken),
            cancellationToken: cancellationToken);

        var attachmentList = attachments.ToList();
        _logger.LogInformation("Fetched {Count} attachments for COURSE_ID: {CourseId}", attachmentList.Count, courseId);
        return attachmentList.Select(mapper.Map<RespondAttachmentDto>).ToList();
    }

    public async Task<RespondAttachmentDto> CreateCourseAttachmentAsync(Guid courseId, RequestAttachmentDto attachmentDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating attachment '{Name}' for COURSE_ID: {CourseId}", attachmentDto.Name, courseId);
        
        var attachment = Attachment.CreateForCourse(courseId, attachmentDto.Key, attachmentDto.Name, attachmentDto.Url);
        
        var addedAttachment = await ExecuteTransactionAsync(
            () => mediator.Send(new CreateAttachmentCommand(attachment), cancellationToken),
            cancellationToken: cancellationToken);
        
        _logger.LogInformation("Created attachment ID: {AttachmentId} for COURSE_ID: {CourseId}", addedAttachment.Id, courseId);
        return mapper.Map<RespondAttachmentDto>(addedAttachment);
    }

    public async Task DeleteCourseAttachmentAsync(Guid attachmentId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting attachment ID: {AttachmentId}", attachmentId);
        
        await ExecuteTransactionAsync(
            () => mediator.Send(new DeleteAttachmentCommand(attachmentId), cancellationToken),
            cancellationToken: cancellationToken);
        
        _logger.LogInformation("Deleted attachment ID: {AttachmentId}", attachmentId);
    }
}