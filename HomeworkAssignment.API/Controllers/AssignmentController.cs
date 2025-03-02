using FluentValidation;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
using HomeworkAssignment.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeworkAssignment.Controllers;

[Authorize]
[Produces("application/json")]
[ApiController]
public class AssignmentController : ControllerBase
{
    private readonly IAssignmentService _assignmentService;

    public AssignmentController(IAssignmentService assignmentService)
    {
        _assignmentService = assignmentService;
    }

    [HttpGet("/api/courses/${courseId:guid}/chapters/${chapterId:guid}/assignments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RespondAssignmentDto>> Get(
        [FromBody] RequestAssignmentFilterParameters filterParameters,
        [FromServices] IValidator<RequestAssignmentFilterParameters> validator,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await validator.ValidateAsync(filterParameters, cancellationToken);
        if (!validationResult.IsValid) return StatusCode(StatusCodes.Status400BadRequest, validationResult.Errors);

        var response = await _assignmentService.GetAssignmentsAsync(filterParameters, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, response);
    }

    [HttpGet("/api/courses/${courseId:guid}/chapters/${chapterId:guid}/assignments/${assignmentId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RespondAssignmentDto>> Get(
        Guid chapterId,
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _assignmentService.GetAssignmentByIdAsync(chapterId, id, cancellationToken);
        if (result == null) return StatusCode(StatusCodes.Status404NotFound);

        return StatusCode(StatusCodes.Status200OK, result);
    }

    [HttpPost("/api/courses/${courseId:guid}/chapters/${chapterId:guid}/assignments")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Guid>> Create(
        Guid userId, 
        Guid chapterId,
        [FromBody] RequestAssignmentDto request,
        [FromServices] IValidator<RequestAssignmentDto> validator,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) return StatusCode(StatusCodes.Status400BadRequest, validationResult.Errors);

        var result = await _assignmentService.CreateAssignmentAsync(userId, chapterId, request, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpDelete("/api/courses/${courseId:guid}/chapters/${chapterId:guid}/assignments/${assignmentId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Guid>> Delete(
        Guid userId, 
        Guid courseId, 
        Guid chapterId, 
        Guid assignmentId,
        CancellationToken cancellationToken = default
    )
    {
        await _assignmentService.DeleteAssignmentAsync(userId, courseId, chapterId, assignmentId, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, assignmentId);
    }

    [HttpPatch("/api/courses/${courseId:guid}/chapters/${chapterId:guid}/assignments/${assignmentId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RespondAssignmentDto>> Update(
        Guid userId, 
        Guid chapterId, 
        Guid assignmentId,
        [FromBody] RequestAssignmentDto request,
        [FromServices] IValidator<RequestAssignmentDto> validator,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) return StatusCode(StatusCodes.Status400BadRequest, validationResult.Errors);

        var response = await _assignmentService.UpdateAssignmentAsync(userId, chapterId, assignmentId, request, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, response);
    }
    
    [HttpPatch("/api/courses/${courseId:guid}/chapters/${chapterId:guid}/assignments/${assignmentId:guid}/publish")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Guid>> Publish(
        Guid userId, 
        Guid chapterId, 
        Guid assignmentId,
        CancellationToken cancellationToken = default
    )
    {
        await _assignmentService.PublishAssignmentAsync(userId, chapterId, assignmentId, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, assignmentId);
    }
    
    [HttpPatch("/api/courses/${courseId:guid}/chapters/${chapterId:guid}/assignments/${assignmentId:guid}/unpublish")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Guid>> Unpublish(
        Guid userId, 
        Guid courseId, 
        Guid chapterId, 
        Guid assignmentId,
        CancellationToken cancellationToken = default
    )
    {
        await _assignmentService.UnpublishAssignmentAsync(userId, courseId, chapterId, assignmentId, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, assignmentId);
    }
}