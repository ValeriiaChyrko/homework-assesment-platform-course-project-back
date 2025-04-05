using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;

namespace HomeworkAssignment.Application.Abstractions.ChapterRelated;

public interface IChapterService
{
    Task<RespondChapterDto> CreateChapterAsync(Guid courseId, RequestCreateChapterDto createChapterDto,
        CancellationToken cancellationToken = default);
    
    Task<RespondChapterDto> UpdateChapterAsync(Guid courseId, Guid chapterId, RequestPartialChapterDto chapterDto,
        CancellationToken cancellationToken = default);
    
    Task<RespondChapterWithAssignmentsDto?> GetChapterByIdAsync(Guid courseId, Guid chapterId,
        CancellationToken cancellationToken = default);
    
    Task<RespondChapterDto?> GetFirstChapterByCourseIdAsync(Guid courseId,
        CancellationToken cancellationToken = default);
    
    Task<RespondChapterDto> PublishChapterAsync(Guid courseId, Guid chapterId, 
        CancellationToken cancellationToken = default);
    
    Task<RespondChapterDto> UnpublishChapterAsync(Guid userId, Guid courseId, Guid chapterId, 
        CancellationToken cancellationToken = default);
    
    Task DeleteChapterAsync(Guid userId, Guid courseId, Guid chapterId, CancellationToken cancellationToken = default);
    
    Task ReorderChapterAsync(Guid userId, Guid courseId, IEnumerable<RequestReorderChapterDto> chapterDtos, 
        CancellationToken cancellationToken = default);
}