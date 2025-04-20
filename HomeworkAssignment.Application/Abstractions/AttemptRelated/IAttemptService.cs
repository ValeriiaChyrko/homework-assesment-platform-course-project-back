using HomeAssignment.DTOs.RequestDTOs.AttemptRelated;
using HomeAssignment.DTOs.RespondDTOs.AttemptRelated;

namespace HomeworkAssignment.Application.Abstractions.AttemptRelated;

public interface IAttemptService
{
    Task<RespondAttemptDto> CreateAttemptAsync(Guid userId, Guid assignmentId,
        CancellationToken cancellationToken = default);

    Task<RespondAttemptDto?> UpdateAttemptAsync(Guid userId, Guid assignmentId, Guid attemptId,
        RequestPartialAttemptDto attemptDto,
        CancellationToken cancellationToken = default);

    Task<RespondAttemptDto?> SubmitAttemptAsync(Guid userId, Guid assignmentId, Guid attemptId,
        RequestSubmitAttemptDto submitAttemptDto,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<RespondAttemptDto>> GetAttemptsByAssignmentIdAsync(
        Guid userId, Guid assignmentId,
        CancellationToken cancellationToken = default);
}