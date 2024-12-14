using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;

namespace HomeworkAssignment.Application.Abstractions;

public interface IAttemptService
{
    Task<RespondAttemptDto> CreateAttemptAsync(RequestAttemptDto attemptDto,
        CancellationToken cancellationToken = default);

    Task<RespondAttemptDto> UpdateAttemptAsync(Guid id, RequestAttemptDto attemptDto,
        CancellationToken cancellationToken = default);

    Task DeleteAttemptAsync(Guid id, CancellationToken cancellationToken = default);
    Task<RespondAttemptDto?> GetAttemptByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<RespondAttemptDto>> GetAttemptsByAssignmentIdAsync(Guid assignmentId,
        CancellationToken cancellationToken = default);

    Task<RespondAttemptDto> GetLastAttemptByAssignmentIdAsync(Guid assignmentId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<RespondAttemptDto>> GetStudentAttemptsAsync(Guid assignmentId, Guid studentId,
        CancellationToken cancellationToken = default);
}