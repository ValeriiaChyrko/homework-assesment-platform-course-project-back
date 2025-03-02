using HomeAssignment.DTOs.RespondDTOs;
using HomeworkAssignment.Application.Abstractions;

namespace HomeworkAssignment.Application.Implementations;

public class ChapterService : IChapterService
{
    public Task<RespondChapterDto> UnpublishChapterAsync(Guid userId, Guid courseId, Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}