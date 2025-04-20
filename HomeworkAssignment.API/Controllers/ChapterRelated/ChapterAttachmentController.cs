using FluentValidation;
using HomeAssignment.DTOs.RequestDTOs;
using HomeworkAssignment.Application.Abstractions.ChapterRelated;
using HomeworkAssignment.AuthorizationFilters;
using HomeworkAssignment.Controllers.Abstractions;
using HomeworkAssignment.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Hybrid;

namespace HomeworkAssignment.Controllers.ChapterRelated;

[Produces("application/json")]
[Authorize]
[ApiController]
[Route("api/courses/{courseId:guid}/chapters/{chapterId:guid}/attachments")]
public class ChapterAttachmentController(
    IChapterAttachmentService service,
    HybridCache cache,
    ICacheKeyManager cacheKeyManager) : BaseController
{
    private readonly HybridCacheEntryOptions _cacheOptions = new()
    {
        LocalCacheExpiration = TimeSpan.FromMinutes(10),
        Expiration = TimeSpan.FromMinutes(20)
    };

    /// <summary>
    ///     Getting attachments for a chapter in a course.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAttachments(Guid courseId, Guid chapterId,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = cacheKeyManager.ChapterAttachments(courseId, chapterId);

        var cachedAttachments = await cache.GetOrCreateAsync(
            cacheKey,
            async _ => await service.GetChapterAttachmentsAsync(courseId, chapterId, cancellationToken),
            _cacheOptions,
            [cacheKeyManager.ChapterSingleGroup(courseId, chapterId)],
            cancellationToken);

        return Ok(cachedAttachments);
    }

    /// <summary>
    ///     Creating an attachment for a chapter in a course.
    /// </summary>
    [HttpPost]
    [TeacherOnly]
    public async Task<IActionResult> CreateAttachment(Guid courseId, Guid chapterId,
        [FromBody] RequestAttachmentDto request,
        [FromServices] IValidator<RequestAttachmentDto> validator,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var attachment = await service.CreateChapterAttachmentAsync(courseId, chapterId, request, cancellationToken);

        await cache.RemoveByTagAsync(cacheKeyManager.ChapterSingleGroup(courseId, chapterId), cancellationToken);
        return Ok(attachment);
    }

    /// <summary>
    ///     Deleting an attachment for a chapter in a course.
    /// </summary>
    [HttpDelete("{attachmentId:guid}")]
    [TeacherOnly]
    public async Task<IActionResult> DeleteAttachment(Guid courseId, Guid chapterId, Guid attachmentId,
        CancellationToken cancellationToken = default)
    {
        await service.DeleteChapterAttachmentAsync(attachmentId, cancellationToken);

        await cache.RemoveByTagAsync(cacheKeyManager.ChapterSingleGroup(courseId, chapterId), cancellationToken);
        return Ok(attachmentId);
    }
}