using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RequestDTOs.AttachmentRelated;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.DTOs.RespondDTOs.AttachmentRelated;

namespace HomeworkAssignment.Application.Abstractions.ChapterRelated;

public interface IChapterAttachmentService
{
    Task<IReadOnlyList<RespondAttachmentDto>> GetChapterAttachmentsAsync(Guid courseId, Guid chapterId,
        CancellationToken cancellationToken = default);

    Task<RespondAttachmentDto> CreateChapterAttachmentAsync(Guid courseId, Guid chapterId,
        RequestAttachmentDto attachmentDto,
        CancellationToken cancellationToken = default);

    Task DeleteChapterAttachmentAsync(Guid attachmentId, CancellationToken cancellationToken = default);
}