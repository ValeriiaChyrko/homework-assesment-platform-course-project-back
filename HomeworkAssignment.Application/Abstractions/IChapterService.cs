using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.DTOs.SharedDTOs;

namespace HomeworkAssignment.Application.Abstractions;

public interface IChapterService
{
    Task<RespondChapterDto> CreateChapterAsync(Guid userId, Guid courseId, RequestChapterDto chapterDto,
        CancellationToken cancellationToken = default);
    Task<RespondChapterDto> UpdateChapterAsync(Guid userId, Guid chapterId, Guid courseId, RequestChapterDto chapterDto,
        CancellationToken cancellationToken = default);
    Task DeleteChapterAsync(Guid userId, Guid courseId, Guid chapterId, CancellationToken cancellationToken = default);
    
    Task ReorderChapterAsync(Guid userId, Guid courseId, IEnumerable<RespondChapterDto> chapterDtos, 
        CancellationToken cancellationToken = default);
    
    Task<RespondChapterDto> PublishChapterAsync(Guid userId, Guid courseId, Guid chapterId, 
        CancellationToken cancellationToken = default);
    Task<RespondChapterDto> UnpublishChapterAsync(Guid userId, Guid courseId, Guid chapterId, 
        CancellationToken cancellationToken = default);
    
    Task<PagedList<RespondChapterDto>> GetChaptersAsync(
        RequestChapterFilterParameters filterParameters,
        CancellationToken cancellationToken = default);

    Task<RespondChapterDto?> GetChapterByIdAsync(Guid chapterId,
        CancellationToken cancellationToken = default);
}