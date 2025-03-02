using HomeAssignment.DTOs.RespondDTOs;

namespace HomeworkAssignment.Application.Abstractions;

public interface IChapterService
{
    Task<RespondChapterDto> UnpublishChapterAsync(Guid userId, Guid courseId, Guid id, CancellationToken cancellationToken = default);
}