using HomeAssignment.DTOs.RequestDTOs.AssignmentRelated;
using HomeAssignment.DTOs.RespondDTOs.AssignmentRelated;

namespace HomeworkAssignment.Application.Abstractions.AssignmentRelated;

public interface IAssignmentService
{
    Task<RespondAssignmentDto> CreateAssignmentAsync(Guid userId, Guid chapterId,
        RequestCreateAssignmentDto createAssignmentDto,
        CancellationToken cancellationToken = default);

    Task<RespondAssignmentDto> UpdateAssignmentAsync(Guid userId, Guid chapterId, Guid assignmentId,
        RequestPartialAssignmentDto assignmentDto,
        CancellationToken cancellationToken = default);

    Task DeleteAssignmentAsync(Guid userId, Guid courseId, Guid chapterId, Guid assignmentId,
        CancellationToken cancellationToken = default);

    Task ReorderAssignmentAsync(Guid userId, Guid courseId, Guid chapterId,
        IEnumerable<RequestReorderAssignmentDto> assignmentDtos,
        CancellationToken cancellationToken = default);

    Task<RespondAssignmentDto> PublishAssignmentAsync(Guid userId, Guid chapterId, Guid assignmentId,
        CancellationToken cancellationToken = default);

    Task<RespondAssignmentDto> UnpublishAssignmentAsync(Guid userId, Guid courseId, Guid chapterId, Guid assignmentId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<RespondAssignmentDto>> GetAssignmentsAsync(Guid chapterId,
        CancellationToken cancellationToken = default);

    Task<RespondAssignmentDto?> GetAssignmentByIdAsync(Guid chapterId, Guid assignmentId,
        CancellationToken cancellationToken = default);
    
    Task<RespondAssignmentAnalyticsDto?> GetAssignmentAnalyticsAsync(Guid chapterId, Guid assignmentId,
        CancellationToken cancellationToken = default);
}