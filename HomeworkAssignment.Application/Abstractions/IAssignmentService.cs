using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.DTOs.SharedDTOs;

namespace HomeworkAssignment.Application.Abstractions;

public interface IAssignmentService
{
    Task<RespondAssignmentDto> CreateAssignmentAsync(Guid userId, Guid chapterId, RequestAssignmentDto assignmentDto,
        CancellationToken cancellationToken = default);
    Task<RespondAssignmentDto> UpdateAssignmentAsync(Guid userId, Guid chapterId, Guid assignmentId, RequestAssignmentDto assignmentDto,
        CancellationToken cancellationToken = default);
    Task DeleteAssignmentAsync(Guid userId, Guid courseId, Guid chapterId, Guid assignmentId, CancellationToken cancellationToken = default);
    
    Task<RespondAssignmentDto> PublishAssignmentAsync(Guid userId, Guid chapterId, Guid assignmentId, 
        CancellationToken cancellationToken = default);
    Task<RespondAssignmentDto> UnpublishAssignmentAsync(Guid userId, Guid courseId, Guid chapterId, Guid assignmentId, 
        CancellationToken cancellationToken = default);

   
    Task<PagedList<RespondAssignmentDto>> GetAssignmentsAsync(
        RequestAssignmentFilterParameters filterParameters,
        CancellationToken cancellationToken = default);

    Task<RespondAssignmentDto?> GetAssignmentByIdAsync(Guid chapterId, Guid assignmentId,
        CancellationToken cancellationToken = default);
}