using FluentValidation;
using HomeAssignment.DTOs.RequestDTOs;
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
[Route("api/courses/{courseId:guid}/attachments")]
public class CourseAttachmentController(ICourseAttachmentService service, HybridCache cache) : BaseController
{
    private readonly HybridCacheEntryOptions _cacheOptions = new()
    {
        LocalCacheExpiration = TimeSpan.FromMinutes(5),
        Expiration = TimeSpan.FromMinutes(10)
    };

    /// <summary>
    /// Getting attachments for a course.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAttachments(Guid courseId, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"attachments-{courseId}";
        
        var cachedAttachments = await cache.GetOrCreateAsync(cacheKey, 
            async _ => await service.GetCourseAttachmentsAsync(courseId, cancellationToken),
            _cacheOptions, cancellationToken: cancellationToken);

        return Ok(cachedAttachments);
    }

    /// <summary>
    /// Creating an attachment for a course.
    /// </summary>
    [HttpPost]
    [TeacherOnly]
    public async Task<IActionResult> CreateAttachment(Guid courseId,
        [FromBody] RequestAttachmentDto request, 
        [FromServices] IValidator<RequestAttachmentDto> validator, 
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
        
        var attachment = await service.CreateCourseAttachmentAsync(courseId, request, cancellationToken);
        
        var cacheKey = $"attachments-{courseId}";
        await cache.RemoveAsync(cacheKey, cancellationToken);
        
        return Ok(attachment);
    }
    
    /// <summary>
    /// Deleting an attachment for a course.
    /// </summary>
    [HttpDelete("{attachmentId:guid}")]
    [TeacherOnly]
    public async Task<IActionResult> DeleteAttachment(Guid courseId, Guid attachmentId, CancellationToken cancellationToken = default)
    {
        await service.DeleteCourseAttachmentAsync(attachmentId, cancellationToken);
        
        var cacheKey = $"attachments-{courseId}";
        await cache.RemoveAsync(cacheKey, cancellationToken);
    
        return Ok(attachmentId);
    }
}
