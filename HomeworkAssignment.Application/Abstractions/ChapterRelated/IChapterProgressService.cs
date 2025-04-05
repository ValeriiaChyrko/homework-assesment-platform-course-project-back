using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;

namespace HomeworkAssignment.Application.Abstractions.ChapterRelated;

public interface IChapterProgressService
{
    Task<RespondChapterUserProgressDto?> GetProgressAsync(Guid userId, Guid courseId, Guid chapterId,
        CancellationToken cancellationToken = default);
    
    Task<RespondChapterUserProgressDto> UpdateProgressAsync(Guid userId, Guid courseId, Guid chapterId, RequestChapterUserProgressDto chapterUserProgressDto,
        CancellationToken cancellationToken = default);
}