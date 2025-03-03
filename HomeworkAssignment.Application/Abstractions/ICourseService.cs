using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.DTOs.SharedDTOs;

namespace HomeworkAssignment.Application.Abstractions;

public interface ICourseService
{
    Task<RespondCourseDto> CreateCourseAsync(Guid userId, RequestCourseDto courseDto,
        CancellationToken cancellationToken = default);
    Task<RespondCourseDto> UpdateCourseAsync(Guid userId, Guid courseId, RequestCourseDto courseDto,
        CancellationToken cancellationToken = default);
    Task DeleteCourseAsync(Guid userId, Guid courseId, CancellationToken cancellationToken = default);
    
    Task<RespondCourseDto> PublishCourseAsync(Guid userId, Guid courseId,
        CancellationToken cancellationToken = default);
    Task<RespondCourseDto> UnpublishCourseAsync(Guid userId, Guid courseId,
        CancellationToken cancellationToken = default);
    
    Task<RespondEnrollmentDto> EnrollAsync(Guid userId, Guid courseId,
        CancellationToken cancellationToken = default);
    
    Task<PagedList<RespondCourseDto>> GetCoursesAsync(
        RequestCourseFilterParameters filterParameters,
        CancellationToken cancellationToken = default);

    Task<RespondCourseDto?> GetCourseByIdAsync(Guid courseId, 
        CancellationToken cancellationToken = default);
}