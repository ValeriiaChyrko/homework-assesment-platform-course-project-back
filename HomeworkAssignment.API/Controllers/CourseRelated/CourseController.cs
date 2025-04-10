using FluentValidation;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RequestDTOs.CourseRelated;
using HomeworkAssignment.Application.Abstractions.CourseRelated;
using HomeworkAssignment.AuthorizationFilters;
using HomeworkAssignment.Controllers.Abstractions;
using HomeworkAssignment.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Hybrid;

namespace HomeworkAssignment.Controllers.CourseRelated;

[Produces("application/json")]
[Authorize]
[ApiController]
[Route("api/courses")]
public class CourseController(ICourseService service, HybridCache cache, ICacheKeyManager cacheKeyManager) : BaseController
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
    public async Task<IActionResult> GetCourseList([FromQuery] RequestCourseFilterParameters filterParameters,
        [FromServices] IValidator<RequestCourseFilterParameters> validator,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(filterParameters, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var userId = GetUserId();
        var cacheKey = cacheKeyManager.CourseList(userId, filterParameters);
        
        var cachedCourses = await cache.GetOrCreateAsync(
            key:cacheKey, 
            async _ => await service.GetCoursesFullInfoAsync(filterParameters, userId, cancellationToken),
            options:_cacheOptions,
            tags: [cacheKeyManager.CourseListGroup(userId)],
            cancellationToken: cancellationToken);

        return Ok(cachedCourses);
    }

    /// <summary>
    /// Retrieves a list of created courses based on user.
    /// </summary>
    [HttpGet("owned")]
    [TeacherOnly]
    public async Task<IActionResult> GetByOwnerCourseList([FromQuery] RequestCourseFilterParameters filterParameters,
        [FromServices] IValidator<RequestCourseFilterParameters> validator,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(filterParameters, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var userId = GetUserId();
        var cacheKey = cacheKeyManager.CourseOwned(userId);
        
        var cachedCourses = await cache.GetOrCreateAsync(
            key:cacheKey,
            async _ => await service.GetUserCoursesFullInfoAsync(filterParameters, userId, cancellationToken),
            options:_cacheOptions, 
            tags: [cacheKeyManager.CourseListGroup(userId)],
            cancellationToken: cancellationToken);
        return Ok(cachedCourses);
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
        var cacheKey = cacheKeyManager.CourseSingle(courseId);
        var cachedCourse = await cache.GetOrCreateAsync(
            key:cacheKey,
            async _ => await service.GetSingleCourseFullInfoAsync(filterParameters, userId, courseId, cancellationToken),
            options:_cacheOptions, 
            tags: [cacheKeyManager.CourseSingleGroup(courseId)],
            cancellationToken: cancellationToken);
        
        if (cachedCourse == null) return NotFound(courseId);
        return Ok(cachedCourse);
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
        
        await cache.RemoveByTagAsync(cacheKeyManager.CourseListGroup(userId), cancellationToken);
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
        var result = await service.UpdateCourseAsync(userId, courseId, request, cancellationToken);
        
        await cache.RemoveAsync(cacheKeyManager.CourseSingle(courseId), cancellationToken);
        await cache.RemoveByTagAsync(cacheKeyManager.CourseSingleGroup(courseId), cancellationToken);
        return Ok(result);
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
        
        await cache.RemoveByTagAsync(cacheKeyManager.CourseSingleGroup(courseId), cancellationToken);
        await cache.RemoveByTagAsync(cacheKeyManager.CourseListGroup(userId), cancellationToken);
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
        
        await cache.RemoveByTagAsync(cacheKeyManager.CourseSingleGroup(courseId), cancellationToken);
        await cache.RemoveByTagAsync(cacheKeyManager.CourseListGroup(userId), cancellationToken);
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
        
        await cache.RemoveByTagAsync(cacheKeyManager.CourseSingleGroup(courseId), cancellationToken);
        await cache.RemoveByTagAsync(cacheKeyManager.CourseListGroup(userId), cancellationToken);
        return Ok(courseId);
    }
}