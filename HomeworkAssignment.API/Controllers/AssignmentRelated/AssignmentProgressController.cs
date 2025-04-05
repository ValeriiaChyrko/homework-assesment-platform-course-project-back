using HomeAssignment.DTOs.RespondDTOs;
using HomeworkAssignment.Application.Abstractions.AssignmentRelated;
using HomeworkAssignment.Controllers.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeworkAssignment.Controllers.AssignmentRelated;

[Produces("application/json")]
[Authorize]
[ApiController]
[Route("api/courses/{courseId:guid}/chapters/{chapterId:guid}/assignments/{assignmentId:guid}/attempts")]
public class AssignmentProgressController(IAssignmentProgressService service) : BaseController
{
    
    /// <summary>
    /// Obtaining student progress on an assignment.
    /// </summary>
    [HttpGet("progress")]
    public async Task<ActionResult<RespondAssignmentUserProgressDto>> GetProgress(Guid assignmentId,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await service.GetProgressByAssignmentIdAsync(userId, assignmentId, cancellationToken);
        return Ok(result);
    }
    
    /// <summary>
    /// Completion of the task.
    /// </summary>
    [HttpPatch("{attemptId:guid}/finish")]
    public async Task<ActionResult<IReadOnlyList<string>>> FinishAssignment(Guid assignmentId,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await service.UpdateProgressAsync(userId, assignmentId, true, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Retrying the task.
    /// </summary>
    [HttpPatch("{attemptId:guid}/restore")]
    public async Task<ActionResult<IReadOnlyList<string>>> RestoreAssignment(Guid assignmentId,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await service.UpdateProgressAsync(userId, assignmentId, false, cancellationToken);
        return Ok(result);
    }
}