using HomeAssignment.DTOs.RequestDTOs.ChapterRelated;
using HomeworkAssignment.Application.Abstractions.ChapterRelated;
using HomeworkAssignment.Controllers.Abstractions;
using HomeworkAssignment.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Hybrid;

namespace HomeworkAssignment.Controllers.ChapterRelated;

[Produces("application/json")]
[Authorize]
[ApiController]
[Route("api/courses/{courseId:guid}/chapters/{chapterId:guid}/progress")]
public class ChapterProgressController(
    IChapterProgressService service,
    HybridCache cache,
    ICacheKeyManager cacheKeyManager) : BaseController
{
    private readonly HybridCacheEntryOptions _cacheOptions = new()
    {
        LocalCacheExpiration = TimeSpan.FromMinutes(2),
        Expiration = TimeSpan.FromMinutes(5)
    };

    /// <summary>
    ///     Getting progress for a specific chapter of a course by user.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetProgress(Guid courseId, Guid chapterId,
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserId();
        var cacheKey = cacheKeyManager.ChapterProgress(userId, courseId, chapterId);

        var cachedProgress = await cache.GetOrCreateAsync(
            cacheKey,
            async _ => await service.GetProgressAsync(userId, courseId, chapterId, cancellationToken),
            _cacheOptions,
            [cacheKeyManager.ChapterSingleGroup(courseId, chapterId)],
            cancellationToken);

        return Ok(cachedProgress);
    }

    /// <summary>
    ///     Updating progress for a specific chapter of a course by user.
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> UpdateProgress(Guid courseId, Guid chapterId,
        [FromBody] RequestChapterUserProgressDto request,
        CancellationToken cancellationToken = default
    )
    {
        var userId = GetUserId();
        await service.UpdateProgressAsync(userId, courseId, chapterId, request, cancellationToken);

        await cache.RemoveByTagAsync(cacheKeyManager.ChapterSingleGroup(courseId, chapterId), cancellationToken);
        return Ok(courseId);
    }
}