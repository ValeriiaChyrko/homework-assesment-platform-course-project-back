using FluentValidation;
using HomeAssignment.DTOs.RequestDTOs;
using HomeworkAssignment.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace HomeworkAssignment.Controllers;

[Produces("application/json")]
[ApiController]
public class CourseController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CourseController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet("/api/courses")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RequestCourseDto>> Get(
        [FromBody] RequestCourseFilterParameters filterParameters,
        [FromServices] IValidator<RequestCourseFilterParameters> validator,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await validator.ValidateAsync(filterParameters, cancellationToken);
        if (!validationResult.IsValid) return StatusCode(StatusCodes.Status400BadRequest, validationResult.Errors);

        var response = await _courseService.GetCoursesAsync(filterParameters, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, response);
    }

    [HttpGet("/api/courses/${courseId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RequestCourseDto>> Get(
        Guid courseId,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _courseService.GetCourseByIdAsync(courseId, cancellationToken);
        if (result == null) return StatusCode(StatusCodes.Status404NotFound);

        return StatusCode(StatusCodes.Status200OK, result);
    }

    [HttpPost("/api/courses")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Guid>> Create(
        Guid userId, 
        [FromBody] RequestCourseDto request,
        [FromServices] IValidator<RequestCourseDto> validator,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) return StatusCode(StatusCodes.Status400BadRequest, validationResult.Errors);

        var result = await _courseService.CreateCourseAsync(userId, request, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpDelete("/api/courses/${courseId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Guid>> Delete(
        Guid userId, 
        Guid courseId,
        CancellationToken cancellationToken = default
    )
    {
        await _courseService.DeleteCourseAsync(userId, courseId, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, courseId);
    }

    [HttpPatch("/api/courses/${courseId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RequestCourseDto>> Update(
        Guid userId, 
        Guid courseId, 
        [FromBody] RequestCourseDto request,
        [FromServices] IValidator<RequestCourseDto> validator,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) return StatusCode(StatusCodes.Status400BadRequest, validationResult.Errors);

        var response = await _courseService.UpdateCourseAsync(userId, courseId, request, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, response);
    }
    
    [HttpPatch("/api/courses/${courseId:guid}/publish")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Guid>> Publish(
        Guid userId,
        Guid courseId,
        CancellationToken cancellationToken = default
    )
    {
        await _courseService.PublishCourseAsync(userId, courseId, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, courseId);
    }
    
    [HttpPatch("/api/courses/${courseId:guid}/unpublish")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Guid>> Unpublish(
        Guid userId, 
        Guid courseId, 
        CancellationToken cancellationToken = default
    )
    {
        await _courseService.UnpublishCourseAsync(userId, courseId, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, courseId);
    }
    
    [HttpPatch("/api/courses/${courseId:guid}/enroll")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Guid>> Enroll(
        Guid userId,
        Guid courseId,
        CancellationToken cancellationToken = default
    )
    {
        await _courseService.EnrollAsync(userId, courseId, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, courseId);
    }
}