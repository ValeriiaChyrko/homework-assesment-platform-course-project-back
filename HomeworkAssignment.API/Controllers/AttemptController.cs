using FluentValidation;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
using HomeworkAssignment.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeworkAssignment.Controllers;

[Authorize]
[Route("api/attempts")]
[Produces("application/json")]
[ApiController]
public class AttemptController : ControllerBase
{
    private readonly IAttemptService _attemptService;

    public AttemptController(IAttemptService attemptService)
    {
        _attemptService = attemptService;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RespondAttemptDto>> GetById(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _attemptService.GetAttemptByIdAsync(id, cancellationToken);
        if (result == null) return StatusCode(StatusCodes.Status404NotFound);

        return StatusCode(StatusCodes.Status200OK, result);
    }

    [HttpGet("{assignmentId:guid}/assignment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IReadOnlyList<RespondAttemptDto>>> GetByAssignmentId(
        Guid assignmentId,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _attemptService.GetAttemptsByAssignmentIdAsync(assignmentId, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, result);
    }

    [HttpGet("{assignmentId:guid}/{studentId:guid}/student")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IReadOnlyList<RespondAttemptDto>>> GetByStudentId(
        Guid assignmentId,
        Guid studentId,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _attemptService.GetStudentAttemptsAsync(assignmentId, studentId, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, result);
    }

    [HttpGet("{assignmentId:guid}/assignment/last")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RespondAttemptDto>> GetLastAttemptByAssignmentId(
        Guid assignmentId,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _attemptService.GetLastAttemptByAssignmentIdAsync(assignmentId, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RespondAttemptDto>> Create(
        [FromQuery] RequestAttemptDto request,
        [FromServices] IValidator<RequestAttemptDto> validator,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) return StatusCode(StatusCodes.Status400BadRequest, validationResult.Errors);

        var result = await _attemptService.CreateAttemptAsync(request, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Guid>> Delete(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        await _attemptService.DeleteAttemptAsync(id, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, id);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RespondAttemptDto>> Update(
        Guid id,
        [FromQuery] RequestAttemptDto request,
        [FromServices] IValidator<RequestAttemptDto> validator,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) return StatusCode(StatusCodes.Status400BadRequest, validationResult.Errors);

        var response = await _attemptService.UpdateAttemptAsync(id, request, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, response);
    }
}