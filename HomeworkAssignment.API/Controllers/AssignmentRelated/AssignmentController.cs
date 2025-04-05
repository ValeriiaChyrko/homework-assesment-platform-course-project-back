using FluentValidation;
using HomeAssignment.DTOs.RequestDTOs;
using HomeworkAssignment.Application.Abstractions.AssignmentRelated;
using HomeworkAssignment.AuthorizationFilters;
using HomeworkAssignment.Controllers.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Hybrid;

namespace HomeworkAssignment.Controllers.AssignmentRelated;

/// <summary>
/// Controller for managing assignments within a course.
/// </summary>
[Produces("application/json")]
[Authorize]
[ApiController]
[Route("api/courses/{courseId:guid}/chapters/{chapterId:guid}/assignments")]
public class AssignmentController(IAssignmentService assignmentService, HybridCache cache) : BaseController
{
    private readonly HybridCacheEntryOptions _cacheOptions = new()
    {
        LocalCacheExpiration = TimeSpan.FromMinutes(5),
        Expiration = TimeSpan.FromMinutes(10)
    };

    /// <summary>
    /// Getting a list of assignments for a chapter.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAssignments(Guid courseId, Guid chapterId,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = $"assignments-{courseId}-{chapterId}";
        var cachedAssignments = await cache.GetOrCreateAsync(cacheKey,
            async _ => await assignmentService.GetAssignmentsAsync(chapterId, cancellationToken),
            _cacheOptions, cancellationToken: cancellationToken);

        return Ok(cachedAssignments);
    }

    /// <summary>
    /// Getting an assignment by ID for a chapter.
    /// </summary>
    [HttpGet("{assignmentId:guid}")]
    public async Task<IActionResult> GetAssignment(Guid courseId, Guid chapterId, Guid assignmentId,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = $"assignment-{courseId}-{chapterId}-{assignmentId}";
        var cachedAssignment = await cache.GetOrCreateAsync(cacheKey,
            async _ => await assignmentService.GetAssignmentByIdAsync(chapterId, assignmentId, cancellationToken),
            _cacheOptions, cancellationToken: cancellationToken);

        if (cachedAssignment == null) return NotFound();

        return Ok(cachedAssignment);
    }

    /// <summary>
    /// Creating a new assignment for a chapter.
    /// </summary>
    [HttpPost]
    [TeacherOnly]
    public async Task<IActionResult> CreateAssignment(Guid courseId, Guid chapterId,
        [FromBody] RequestAssignmentDto request,
        [FromServices] IValidator<RequestAssignmentDto> validator,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var assignmentId = await assignmentService.CreateAssignmentAsync(request.UserId, chapterId, request, cancellationToken);
        await cache.RemoveAsync($"assignments-{courseId}-{chapterId}", cancellationToken);

        return CreatedAtAction(nameof(GetAssignment), new { courseId, chapterId, assignmentId }, assignmentId);
    }

    /// <summary>
    /// Updating an assignment for a chapter.
    /// </summary>
    [HttpPatch("{assignmentId:guid}")]
    [TeacherOnly]
    public async Task<IActionResult> UpdateAssignment(Guid courseId, Guid chapterId, Guid assignmentId,
        [FromBody] RequestPartialAssignmentDto request,
        [FromServices] IValidator<RequestPartialAssignmentDto> validator,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var updatedAssignment = await assignmentService.UpdateAssignmentAsync(request.UserId, chapterId, assignmentId, request, cancellationToken);
        await cache.RemoveAsync($"assignments-{courseId}-{chapterId}", cancellationToken);

        return Ok(updatedAssignment);
    }

    /// <summary>
    /// Publishing an assignment.
    /// </summary>
    [HttpPatch("{assignmentId:guid}/publish")]
    [TeacherOnly]
    public async Task<IActionResult> PublishAssignment(Guid courseId, Guid chapterId, Guid assignmentId,
        CancellationToken cancellationToken = default)
    {
        await assignmentService.PublishAssignmentAsync(courseId, chapterId, assignmentId, cancellationToken);
        return Ok(assignmentId);
    }

    /// <summary>
    /// Unpublishing an assignment.
    /// </summary>
    [HttpPatch("{assignmentId:guid}/unpublish")]
    [TeacherOnly]
    public async Task<IActionResult> UnpublishAssignment(Guid courseId, Guid chapterId, Guid assignmentId,
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserId();
        await assignmentService.UnpublishAssignmentAsync(userId, courseId, chapterId, assignmentId, cancellationToken);
        return Ok(assignmentId);
    }

    /// <summary>
    /// Deleting an assignment by ID.
    /// </summary>
    [HttpDelete("{assignmentId:guid}")]
    [TeacherOnly]
    public async Task<IActionResult> DeleteAssignment(Guid courseId, Guid chapterId, Guid assignmentId,
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserId();
        await assignmentService.DeleteAssignmentAsync(userId, courseId, chapterId, assignmentId, cancellationToken);
        await cache.RemoveAsync($"assignments-{courseId}-{chapterId}", cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Reordering assignment for a chapter.
    /// </summary>
    [HttpPut("reorder")]
    [TeacherOnly]
    public async Task<IActionResult> Reorder(Guid courseId, Guid chapterId,
        [FromBody] RequestReorderAssignmentDto[] assignmentDtos, 
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserId();
        await assignmentService.ReorderAssignmentAsync(userId, courseId, chapterId, assignmentDtos, cancellationToken);
        return Ok(courseId);
    }
}