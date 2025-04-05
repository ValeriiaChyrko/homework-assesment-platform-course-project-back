using HomeworkAssignment.Application.Abstractions.CourseRelated;
using HomeworkAssignment.Controllers.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Hybrid;

namespace HomeworkAssignment.Controllers.CourseRelated;

[Produces("application/json")]
[Authorize]
[ApiController]
[Route("api/courses/{courseId:guid}/enrollments")]
public class CourseEnrollmentController(ICourseEnrollmentService service, HybridCache cache) : BaseController
{
    private readonly HybridCacheEntryOptions _cacheOptions = new()
    {
        LocalCacheExpiration = TimeSpan.FromMinutes(5),
        Expiration = TimeSpan.FromMinutes(10)
    };
    
    /// <summary>
    /// Enrolls a user in a course.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Enroll(Guid courseId, CancellationToken cancellationToken = default)
    {
        var userId = GetUserId();
        var result = await service.EnrollAsync(userId, courseId, cancellationToken);
        return Ok(result);
    }
    
    /// <summary>
    /// Withdraws a user from a course.
    /// </summary>
    [HttpPatch("withdraw")]
    public async Task<IActionResult> Withdraw(Guid courseId, CancellationToken cancellationToken = default)
    {
        var userId = GetUserId();
        await service.WithdrawAsync(userId, courseId, cancellationToken);
        return NoContent();
    }
    
    /// <summary>
    /// Gets enrollment details of a user for a course.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetEnrollment(Guid courseId, CancellationToken cancellationToken = default)
    {
        var userId = GetUserId();
        var cacheKey = $"enrollment-{courseId}-{userId}";
        
        var result = await cache.GetOrCreateAsync(cacheKey, 
            async _ => await service.GetEnrollmentAsync(userId, courseId, cancellationToken),
            _cacheOptions, cancellationToken: cancellationToken);
        
        return Ok(result);
    }
    
    /// <summary>
    /// Gets all enrolled courses for a user.
    /// </summary>
    [HttpGet("/all")]
    public async Task<IActionResult> GetEnrollmentWithCoursesAsync(CancellationToken cancellationToken = default)
    {
        var userId = GetUserId();
        var cacheKey = $"enrollments-{userId}";
        
        var response = await cache.GetOrCreateAsync(cacheKey, 
            async _ => await service.GetEnrolledCoursesAsync(userId, cancellationToken),
            _cacheOptions, cancellationToken: cancellationToken);
        
        return Ok(response);
    }
}