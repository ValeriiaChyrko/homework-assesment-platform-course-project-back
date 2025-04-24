using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RequestDTOs.AttachmentRelated;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.DTOs.RespondDTOs.AttachmentRelated;

namespace HomeworkAssignment.Application.Abstractions.CourseRelated;

public interface ICourseAttachmentService
{
    Task<IReadOnlyList<RespondAttachmentDto>> GetCourseAttachmentsAsync(Guid courseId,
        CancellationToken cancellationToken = default);

    Task<RespondAttachmentDto> CreateCourseAttachmentAsync(Guid courseId, RequestAttachmentDto attachmentDto,
        CancellationToken cancellationToken = default);

    Task DeleteCourseAttachmentAsync(Guid attachmentId, CancellationToken cancellationToken = default);
}