using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;

namespace HomeworkAssignment.Application.Abstractions.CourseRelated;

public interface ICourseAttachmentService
{
    Task<IReadOnlyList<RespondAttachmentDto>> GetCourseAttachmentsAsync(Guid courseId,
        CancellationToken cancellationToken = default);

    Task<RespondAttachmentDto> CreateCourseAttachmentAsync(Guid courseId, RequestAttachmentDto attachmentDto,
        CancellationToken cancellationToken = default);

    Task DeleteCourseAttachmentAsync(Guid attachmentId, CancellationToken cancellationToken = default);
}