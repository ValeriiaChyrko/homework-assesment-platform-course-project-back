using FluentValidation;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
using HomeworkAssignment.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace HomeworkAssignment.Controllers;

[Route("api/students")]
[Produces("application/json")]
[ApiController]
public class StudentController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IReadOnlyList<RespondStudentDto>>> Get(
        CancellationToken cancellationToken = default
    )
    {
        var result = await _studentService.GetStudentsAsync(cancellationToken);
        return StatusCode(StatusCodes.Status200OK, result);
    }

    [HttpGet("{githubProfileId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RespondStudentDto>> GetById(
        Guid githubProfileId,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _studentService.GetStudentByIdAsync(githubProfileId, cancellationToken);
        if (result == null) return StatusCode(StatusCodes.Status404NotFound);

        return StatusCode(StatusCodes.Status200OK, result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Guid>> Create(
        [FromQuery] RequestStudentDto request,
        [FromServices] IValidator<RequestStudentDto> validator,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) return StatusCode(StatusCodes.Status400BadRequest, validationResult.Errors);

        var result = await _studentService.CreateStudentAsync(request, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpDelete("{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Guid>> Delete(
        Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        await _studentService.DeleteStudentAsync(userId, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, userId);
    }

    [HttpPut("{userId:guid}/{githubProfileId:guid}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RespondStudentDto>> Update(
        Guid userId,
        Guid githubProfileId,
        [FromQuery] RequestStudentDto request,
        [FromServices] IValidator<RequestStudentDto> validator,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) return StatusCode(StatusCodes.Status400BadRequest, validationResult.Errors);

        var response = await _studentService.UpdateStudentAsync(userId, githubProfileId, request, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, response);
    }
}