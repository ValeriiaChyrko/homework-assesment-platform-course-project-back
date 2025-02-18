using FluentValidation;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
using HomeworkAssignment.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeworkAssignment.Controllers;

[Authorize]
[Route("api/assignments")]
[Produces("application/json")]
[ApiController]
public class AssignmentController : ControllerBase
{
    private readonly IAssignmentService _assignmentService;

    public AssignmentController(IAssignmentService assignmentService)
    {
        _assignmentService = assignmentService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RespondAssignmentDto>> Get(
        [FromQuery] RequestAssignmentFilterParameters filterParameters,
        [FromServices] IValidator<RequestAssignmentFilterParameters> validator,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await validator.ValidateAsync(filterParameters, cancellationToken);
        if (!validationResult.IsValid) return StatusCode(StatusCodes.Status400BadRequest, validationResult.Errors);

        var response = await _assignmentService.GetAssignmentsAsync(filterParameters, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RespondAssignmentDto>> Get(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _assignmentService.GetAssignmentByIdAsync(id, cancellationToken);
        if (result == null) return StatusCode(StatusCodes.Status404NotFound);

        return StatusCode(StatusCodes.Status200OK, result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Guid>> Create(
        [FromQuery] RequestAssignmentDto request,
        [FromServices] IValidator<RequestAssignmentDto> validator,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) return StatusCode(StatusCodes.Status400BadRequest, validationResult.Errors);

        var result = await _assignmentService.CreateAssignmentAsync(request, cancellationToken);
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
        await _assignmentService.DeleteAssignmentAsync(id, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, id);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RespondAssignmentDto>> Update(
        Guid id,
        [FromQuery] RequestAssignmentDto request,
        [FromServices] IValidator<RequestAssignmentDto> validator,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) return StatusCode(StatusCodes.Status400BadRequest, validationResult.Errors);

        var response = await _assignmentService.UpdateAssignmentAsync(id, request, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, response);
    }
}