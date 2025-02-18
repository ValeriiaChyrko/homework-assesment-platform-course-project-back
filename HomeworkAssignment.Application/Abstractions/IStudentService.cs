using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.DTOs.SharedDTOs;

namespace HomeworkAssignment.Application.Abstractions;

public interface IStudentService
{
    Task<RespondStudentDto> CreateStudentAsync(RequestStudentDto studentDto,
        CancellationToken cancellationToken = default);

    Task<RespondStudentDto> UpdateStudentAsync(Guid userId, Guid githubProfileId, RequestStudentDto studentDto,
        CancellationToken cancellationToken = default);

    Task DeleteStudentAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<RespondStudentDto?> GetStudentByIdAsync(Guid githubProfileId,
        CancellationToken cancellationToken = default);

    Task<PagedList<RespondStudentDto>> GetStudentsAsync(RequestUserFilterParameters filterParameters,
        CancellationToken cancellationToken = default);
}