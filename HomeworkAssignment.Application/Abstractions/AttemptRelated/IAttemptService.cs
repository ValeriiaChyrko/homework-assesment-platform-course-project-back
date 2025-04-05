using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;

namespace HomeworkAssignment.Application.Abstractions.AttemptRelated;

public interface IAttemptService
{
    Task<RespondAttemptDto> CreateAttemptAsync(Guid userId, Guid assignmentId, RequestAttemptDto attemptDto,
        CancellationToken cancellationToken = default);
    
    Task<RespondAttemptDto?> UpdateAttemptAsync(Guid userId, Guid assignmentId, Guid attemptId, RequestPartialAttemptDto attemptDto,
        CancellationToken cancellationToken = default);
    
    Task<RespondAttemptDto?> SubmitAttemptAsync(Guid userId, Guid assignmentId, Guid attemptId, RequestAttemptDto attemptDto,
        CancellationToken cancellationToken = default);
    
    Task<IReadOnlyList<RespondAttemptDto>> GetAttemptsByAssignmentIdAsync(
        Guid userId, Guid assignmentId,
        CancellationToken cancellationToken = default);
}