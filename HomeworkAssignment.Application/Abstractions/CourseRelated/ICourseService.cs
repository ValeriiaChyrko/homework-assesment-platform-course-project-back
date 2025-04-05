using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RequestDTOs.CourseRelated;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.DTOs.SharedDTOs;

namespace HomeworkAssignment.Application.Abstractions.CourseRelated;

public interface ICourseService
{
    Task<RespondCourseDto> CreateCourseAsync(Guid userId, RequestCourseDto courseDto,
        CancellationToken cancellationToken = default);
    Task<RespondCourseDto> UpdateCourseAsync(Guid userId, Guid courseId, RequestPartialCourseDto courseDto,
        CancellationToken cancellationToken = default);
    Task DeleteCourseAsync(Guid userId, Guid courseId, CancellationToken cancellationToken = default);
    
    Task<RespondCourseDto> PublishCourseAsync(Guid userId, Guid courseId,
        CancellationToken cancellationToken = default);
    Task<RespondCourseDto> UnpublishCourseAsync(Guid userId, Guid courseId,
        CancellationToken cancellationToken = default);
    
    Task<PagedList<RespondCourseDto>> GetCoursesAsync(
        RequestCourseFilterParameters filterParameters,
        Guid userId,
        CancellationToken cancellationToken = default);
    
    Task<PagedList<RespondCourseWithCategoryDto>> GetCoursesWithCategoryAsync(
        RequestCourseFilterParameters filterParameters,
        CancellationToken cancellationToken = default);
    
    Task<PagedList<RespondCourseWithCategoryProgressDto>> GetCoursesWithCategoryWithProgressAsync(
        RequestCourseFilterParameters filterParameters, Guid userId,
        CancellationToken cancellationToken = default);

    Task<RespondCourseWithCategoryWithProgressDto?> GetCourseWithCategoryWithProgressAsync(Guid courseId, Guid userId,
        CancellationToken cancellationToken = default);

    Task<RespondCourseWithChapters?> GetCourseWithChaptersByIdAsync(Guid courseId,
        CancellationToken cancellationToken = default);
    
    Task<PagedList<RespondCourseWithCategoryProgressDto>> GetEnrolledCoursesWithCategoryWithProgressAsync(
        Guid userId,
        RequestCourseFilterParameters filterParameters,
        CancellationToken cancellationToken = default);

    Task<RespondCourseDto?> GetCourseByIdAsync(Guid courseId, Guid userId, 
        CancellationToken cancellationToken = default);
    
    Task<RespondCourseWithChaptersWithAttachmentsDto?> GetCourseWithChaptersWithAttachmentsByIdAsync(Guid courseId, Guid userId, 
        CancellationToken cancellationToken = default);
}