using HomeAssignment.DTOs.RequestDTOs.ChapterRelated;
using HomeAssignment.DTOs.RespondDTOs.ChapterRelated;

namespace HomeworkAssignment.Application.Abstractions.ChapterRelated;

public interface IChapterService
{
    Task<RespondChapterDto> CreateChapterAsync(Guid courseId, RequestCreateChapterDto createChapterDto,
        CancellationToken cancellationToken = default);

    Task<RespondChapterDto> UpdateChapterAsync(Guid courseId, Guid chapterId, RequestPartialChapterDto chapterDto,
        CancellationToken cancellationToken = default);

    Task<RespondChapterDto> PublishChapterAsync(Guid courseId, Guid chapterId,
        CancellationToken cancellationToken = default);

    Task<RespondChapterDto> UnpublishChapterAsync(Guid userId, Guid courseId, Guid chapterId,
        CancellationToken cancellationToken = default);

    Task DeleteChapterAsync(Guid userId, Guid courseId, Guid chapterId, CancellationToken cancellationToken = default);

    Task ReorderChapterAsync(Guid courseId, IEnumerable<RequestReorderChapterDto> chapterDtos,
        CancellationToken cancellationToken = default);
    
    Task<List<RespondChapterDto>> GetChaptersAsync(Guid courseId, CancellationToken cancellationToken = default);
    
    Task<RespondChapterWithAssignmentsDto?> GetChapterByIdAsync(Guid courseId, Guid chapterId,
        CancellationToken cancellationToken = default);

    Task<RespondChapterDto?> GetFirstChapterByCourseIdAsync(Guid courseId,
        CancellationToken cancellationToken = default);

    Task<RespondChapterDto?> GetNextChapterByChapterIdAsync(Guid courseId, Guid chapterId,
        CancellationToken cancellationToken = default);
}