﻿using HomeAssignment.DTOs.RespondDTOs.AssignmentRelated;

namespace HomeworkAssignment.Application.Abstractions.AssignmentRelated;

public interface IAssignmentProgressService
{
    Task<RespondAssignmentUserProgressDto?> GetProgressByAssignmentIdAsync(
        Guid userId, Guid assignmentId,
        CancellationToken cancellationToken = default);

    Task<RespondAssignmentUserProgressDto> UpdateProgressAsync(Guid userId, Guid assignmentId, bool completed,
        CancellationToken cancellationToken = default);
}