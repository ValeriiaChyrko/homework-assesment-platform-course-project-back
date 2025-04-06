using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RequestDTOs.CourseRelated;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.Persistence.Abstractions;

namespace HomeworkAssignment.Application.Abstractions.CourseRelated;

public interface ICourseService
{
    Task<RespondCourseDto> CreateCourseAsync(Guid userId, RequestCreateCourseDto createCourseDto,
        CancellationToken cancellationToken = default);
    Task<RespondCourseDto> UpdateCourseAsync(Guid userId, Guid courseId, RequestPartialCourseDto courseDto,
        CancellationToken cancellationToken = default);
    Task DeleteCourseAsync(Guid userId, Guid courseId, CancellationToken cancellationToken = default);
    
    Task<RespondCourseDto> PublishCourseAsync(Guid userId, Guid courseId,
        CancellationToken cancellationToken = default);
    Task<RespondCourseDto> UnpublishCourseAsync(Guid userId, Guid courseId,
        CancellationToken cancellationToken = default);
    
    Task<PagedList<RespondCourseFullInfoDto>> GetCoursesFullInfoAsync(
        RequestCourseFilterParameters filterParameters,
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<PagedList<RespondCourseFullInfoDto>> GetUserCoursesFullInfoAsync(
        RequestCourseFilterParameters filterParameters, Guid userId, CancellationToken cancellationToken = default);
    
    Task<RespondCourseFullInfoDto?> GetSingleCourseFullInfoAsync(
        RequestCourseFilterParameters filterParameters, Guid userId, Guid courseId, CancellationToken cancellationToken = default);
}