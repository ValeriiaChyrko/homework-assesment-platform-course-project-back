using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
using HomeworkAssignment.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

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
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IReadOnlyList<RespondStudentDto>>> Get()
    {
        var result = await _studentService.GetStudentsAsync();
        return StatusCode(StatusCodes.Status200OK, result);
    }

    [HttpGet("{githubProfileId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RespondStudentDto>> Get(Guid githubProfileId)
    {
        var result = await _studentService.GetStudentByIdAsync(githubProfileId);
        return StatusCode(StatusCodes.Status200OK, result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Guid>> Create([FromBody] RequestStudentDto request)
    {
        var result = await _studentService.CreateStudentAsync(request);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpDelete("{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Guid>> Delete(Guid userId)
    {
        await _studentService.DeleteStudentAsync(userId);
        return StatusCode(StatusCodes.Status200OK, userId);
    }

    [HttpPut("{userId:guid}/{githubProfileId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RespondStudentDto>> Update(Guid userId, Guid githubProfileId,
        RequestStudentDto request)
    {
        var response = await _studentService.UpdateStudentAsync(userId, githubProfileId, request);
        return StatusCode(StatusCodes.Status200OK, response);
    }
}