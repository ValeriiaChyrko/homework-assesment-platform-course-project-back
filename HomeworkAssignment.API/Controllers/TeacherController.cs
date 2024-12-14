using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
using HomeworkAssignment.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace HomeworkAssignment.Controllers;

[Route("api/teachers")]
[Produces("application/json")]
[ApiController]
public class TeacherController : ControllerBase
{
    private readonly ITeacherService _teacherService;

    public TeacherController(ITeacherService teacherService)
    {
        _teacherService = teacherService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IReadOnlyList<RespondTeacherDto>>> Get()
    {
        var result = await _teacherService.GetTeachersAsync();
        return StatusCode(StatusCodes.Status200OK, result);
    }

    [HttpGet("{githubProfileId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RespondTeacherDto>> Get(Guid githubProfileId)
    {
        var result = await _teacherService.GetTeacherByIdAsync(githubProfileId);
        if (result == null) return StatusCode(StatusCodes.Status404NotFound);

        return StatusCode(StatusCodes.Status200OK, result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Guid>> Create([FromBody] RequestTeacherDto request)
    {
        var result = await _teacherService.CreateTeacherAsync(request);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpDelete("{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Guid>> Delete(Guid userId)
    {
        await _teacherService.DeleteTeacherAsync(userId);
        return StatusCode(StatusCodes.Status200OK, userId);
    }

    [HttpPut("{userId:guid}/{githubProfileId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RespondTeacherDto>> Update(Guid userId, Guid githubProfileId,
        RequestTeacherDto request)
    {
        var response = await _teacherService.UpdateTeacherAsync(userId, githubProfileId, request);
        return StatusCode(StatusCodes.Status200OK, response);
    }
}