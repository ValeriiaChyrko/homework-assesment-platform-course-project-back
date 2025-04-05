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
        var cacheKey = $"courses-{userId}-{JsonSerializer.Serialize(filterParameters)}"; ;
        var result = await cache.GetOrCreateAsync(cacheKey,
            async _ => await service.GetCoursesAsync(filterParameters, userId, cancellationToken),
            _cacheOptions, cancellationToken:cancellationToken);

        return Ok(result);
    }
    
    /// <summary>
    /// Retrieves enrolled courses with evaluation details for a user.
    /// </summary>
    [HttpGet("evaluation")]
    public async Task<IActionResult> GetCoursesFullInfoAsync([FromQuery] Guid userId, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"enrolled-courses-{userId}";
        // var response = await cache.GetOrCreateAsync(cacheKey, 
        //     async _ => await service.GetEnrolledCoursesAsync(userId, cancellationToken),
        //     _cacheOptions, cancellationToken: cancellationToken);
        //
        // return Ok(response);
        return NoContent();
    }
    
    /// <summary>
    /// Retrieves course details by ID.
    /// </summary>
    [HttpGet("{courseId:guid}")]
    public async Task<IActionResult> Get(Guid courseId,
        [FromQuery] RequestSingleCourseFilterParameters filterParameters,
        [FromServices] IValidator<RequestSingleCourseFilterParameters> validator,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(filterParameters, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        var cacheKey = $"course-{courseId}-{filterParameters.OwnerId}";
        // var result = await cache.GetOrCreateAsync(cacheKey, 
        //     async _ => filterParameters.Include?.Contains("chapters") && filterParameters.Include.Contains("attachments")
        //         ? await service.GetCourseWithChaptersWithAttachmentsByIdAsync(courseId, filterParameters.OwnerId, cancellationToken)
        //         : await service.GetCourseByIdAsync(courseId, filterParameters.OwnerId, cancellationToken),
        //     _cacheOptions, cancellationToken: cancellationToken);
        //
        // return result != null ? Ok(result) : NotFound();
        return NoContent();
    }
    
    /// <summary>
    /// Creates a new course.
    /// </summary>
    [HttpPost]
    [TeacherOnly]
    public async Task<IActionResult> Create([FromBody] RequestCourseDto request,
        [FromServices] IValidator<RequestCourseDto> validator,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        var result = await service.CreateCourseAsync(request.UserId, request, cancellationToken);
        return CreatedAtAction(nameof(Get), new { courseId = result.Id }, result);
    }
    
    /// <summary>
    /// Deletes a course by ID.
    /// </summary>
    [HttpDelete("{courseId:guid}")]
    [TeacherOnly]
    public async Task<IActionResult> Delete(Guid courseId, [FromQuery] Guid userId, CancellationToken cancellationToken = default)
    {
        await service.DeleteCourseAsync(userId, courseId, cancellationToken);
        return Ok(courseId);
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
        
        var response = await service.UpdateCourseAsync(request.UserId, courseId, request, cancellationToken);
        return Ok(response);
    }
    
    /// <summary>
    /// Publishes a course.
    /// </summary>
    [HttpPatch("{courseId:guid}/publish")]
    [TeacherOnly]
    public async Task<IActionResult> Publish([FromQuery] Guid userId, Guid courseId, CancellationToken cancellationToken = default)
    {
        await service.PublishCourseAsync(userId, courseId, cancellationToken);
        return Ok(courseId);
    }
    
    /// <summary>
    /// Unpublishes a course.
    /// </summary>
    [HttpPatch("{courseId:guid}/unpublish")]
    [TeacherOnly]
    public async Task<IActionResult> Unpublish([FromQuery] Guid userId, Guid courseId, CancellationToken cancellationToken = default)
    {
        await service.UnpublishCourseAsync(userId, courseId, cancellationToken);
        return Ok(courseId);
    }
}