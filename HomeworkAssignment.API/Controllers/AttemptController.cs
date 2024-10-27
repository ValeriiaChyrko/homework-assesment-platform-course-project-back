using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
using HomeworkAssignment.Application.Abstractions.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

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
    
    [HttpGet("{assignmentId:guid}/assignment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IReadOnlyList<RespondAttemptDto>>> ByAssignmentId(Guid assignmentId)
    {
        var result = await _attemptService.GetAttemptsByAssignmentIdAsync(assignmentId);
        return StatusCode(StatusCodes.Status200OK, result);
    }
    
    [HttpGet("{studentId:guid}/student")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IReadOnlyList<RespondAttemptDto>>> ByStudentId(Guid studentId)
    {
        var result = await _attemptService.GetAttemptsByStudentIdAsync(studentId);
        return StatusCode(StatusCodes.Status200OK, result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RespondAttemptDto>> Get(Guid id)
    {
        var result = await _attemptService.GetAttemptByIdAsync(id);
        return StatusCode(StatusCodes.Status200OK, result);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Guid>> Create([FromBody] RequestAttemptDto request)
    {
        var result = await _attemptService.CreateAttemptAsync(request);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Guid>> Delete(Guid id)
    {
        await _attemptService.DeleteAttemptAsync(id);
        return StatusCode(StatusCodes.Status200OK, id);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RespondAttemptDto>> Update(Guid id, RequestAttemptDto request)
    {
        var response = await _attemptService.UpdateAttemptAsync(id, request);
        return StatusCode(StatusCodes.Status200OK, response);
    }
}