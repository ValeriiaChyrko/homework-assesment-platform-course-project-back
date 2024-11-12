using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;

namespace HomeworkAssignment.Application.Abstractions;

public interface IAssignmentService
{
    Task<RespondAssignmentDto> CreateAssignmentAsync(RequestAssignmentDto assignmentDto,
        CancellationToken cancellationToken = default);

    Task<RespondAssignmentDto> UpdateAssignmentAsync(Guid id, RequestAssignmentDto assignmentDto,
        CancellationToken cancellationToken = default);

    Task DeleteAssignmentAsync(Guid id, CancellationToken cancellationToken = default);
    Task<RespondAssignmentDto?> GetAssignmentByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RespondAssignmentDto>> GetAssignmentsAsync(CancellationToken cancellationToken = default);
}