using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;

namespace HomeworkAssignment.Application.Abstractions;

public interface ITeacherService
{
    Task<RespondTeacherDto> CreateTeacherAsync(RequestTeacherDto teacherDto,
        CancellationToken cancellationToken = default);

    Task<RespondTeacherDto> UpdateTeacherAsync(Guid userId, Guid githubProfileId, RequestTeacherDto teacherDto,
        CancellationToken cancellationToken = default);

    Task DeleteTeacherAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<RespondTeacherDto?> GetTeacherByIdAsync(Guid githubProfileId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<RespondTeacherDto>> GetTeachersAsync(CancellationToken cancellationToken = default);
}