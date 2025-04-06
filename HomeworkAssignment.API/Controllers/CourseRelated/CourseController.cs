using System.Text.Json;
using FluentValidation;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RequestDTOs.CourseRelated;
using HomeworkAssignment.Application.Abstractions.CourseRelated;
using HomeworkAssignment.AuthorizationFilters;
using HomeworkAssignment.Controllers.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Hybrid;

namespace HomeworkAssignment.Controllers.CourseRelated;

[Produces("application/json")]
[Authorize]
[ApiController]
[Route("api/courses")]
public class CourseController(ICourseService service, HybridCache cache) : BaseController
{
    private readonly HybridCacheEntryOptions _cacheOptions = new()
    {
        LocalCacheExpiration = TimeSpan.FromMinutes(5),
        Expiration = TimeSpan.FromMinutes(10)
    };

    /// <summary>
    /// Retrieves a list of courses based on filter parameters.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] RequestCourseFilterParameters filterParameters,
        [FromServices] IValidator<RequestCourseFilterParameters> validator,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(filterParameters, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var userId = GetUserId();
        var cacheKey = $"courses-{userId}-{JsonSerializer.Serialize(filterParameters)}";
        ;
        var result = await cache.GetOrCreateAsync(cacheKey,
            async _ => await service.GetCoursesFullInfoAsync(filterParameters, userId, cancellationToken),
            _cacheOptions, cancellationToken: cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a list of created courses based on user.
    /// </summary>
    [HttpGet("owned")]
    [TeacherOnly]
    public async Task<IActionResult> GetByOwner([FromQuery] RequestCourseFilterParameters filterParameters,
        [FromServices] IValidator<RequestCourseFilterParameters> validator,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(filterParameters, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var userId = GetUserId();
        var cacheKey = $"courses-owned-by-{userId}";
        
        var result = await cache.GetOrCreateAsync(cacheKey,
            async _ => await service.GetUserCoursesFullInfoAsync(filterParameters, userId, cancellationToken),
            _cacheOptions, cancellationToken: cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a single course based on filter parameters.
    /// </summary>
    [HttpGet("{courseId:guid}")]
    public async Task<IActionResult> Get(Guid courseId,
        [FromQuery] RequestCourseFilterParameters filterParameters,
        [FromServices] IValidator<RequestCourseFilterParameters> validator,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(filterParameters, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var userId = GetUserId();
        var cacheKey = $"course-{courseId}-{JsonSerializer.Serialize(filterParameters)}";
        // var result = await cache.GetOrCreateAsync(cacheKey,
        //     async _ => await service.GetSingleCourseFullInfoAsync(filterParameters, userId, courseId,
        //         cancellationToken),
        //     _cacheOptions, cancellationToken: cancellationToken);
        var result = await service.GetSingleCourseFullInfoAsync(filterParameters, userId, courseId, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new course.
    /// </summary>
    [HttpPost]
    [TeacherOnly]
    public async Task<IActionResult> Create([FromBody] RequestCreateCourseDto requestCreate,
        [FromServices] IValidator<RequestCreateCourseDto> validator,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(requestCreate, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var userId = GetUserId();
        var result = await service.CreateCourseAsync(userId, requestCreate, cancellationToken);
        
        var cacheKey = $"courses-owned-by-{userId}";
        await cache.RemoveAsync(cacheKey, cancellationToken);

        return CreatedAtAction(nameof(Get), new { courseId = result.Id }, result);
    }

    /// <summary>
    /// Updates an existing course.
    /// </summary>
    [HttpPatch("{courseId:guid}")]
    [TeacherOnly]
    public async Task<IActionResult> Update(Guid courseId, [FromBody] RequestPartialCourseDto request,
        [FromServices] IValidator<RequestPartialCourseDto> validator,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var userId = GetUserId();
        var response = await service.UpdateCourseAsync(userId, courseId, request, cancellationToken);
        
        var cacheKey =
            $"course-{courseId}-{JsonSerializer.Serialize(new RequestCourseFilterParameters())}"; 
        await cache.RemoveAsync(cacheKey, cancellationToken);
        
        cacheKey = $"courses-owned-by-{userId}";
        await cache.RemoveAsync(cacheKey, cancellationToken);

        return Ok(response);
    }

    /// <summary>
    /// Deletes a course by ID.
    /// </summary>
    [HttpDelete("{courseId:guid}")]
    [TeacherOnly]
    public async Task<IActionResult> Delete(Guid courseId, [FromQuery] Guid userId,
        CancellationToken cancellationToken = default)
    {
        await service.DeleteCourseAsync(userId, courseId, cancellationToken);
        
        var cacheKey = $"courses-owned-by-{userId}";
        await cache.RemoveAsync(cacheKey, cancellationToken);

        cacheKey =
            $"course-{courseId}-{JsonSerializer.Serialize(new RequestCourseFilterParameters())}"; 
        await cache.RemoveAsync(cacheKey, cancellationToken);

        return Ok(courseId);
    }

    /// <summary>
    /// Publishes a course.
    /// </summary>
    [HttpPatch("{courseId:guid}/publish")]
    [TeacherOnly]
    public async Task<IActionResult> Publish(Guid courseId, CancellationToken cancellationToken = default)
    {
        var userId = GetUserId();
        await service.PublishCourseAsync(userId, courseId, cancellationToken);
        return Ok(courseId);
    }

    /// <summary>
    /// Unpublishes a course.
    /// </summary>
    [HttpPatch("{courseId:guid}/unpublish")]
    [TeacherOnly]
    public async Task<IActionResult> Unpublish(Guid courseId, CancellationToken cancellationToken = default)
    {
        var userId = GetUserId();
        await service.UnpublishCourseAsync(userId, courseId, cancellationToken);
        return NoContent();
    }
}