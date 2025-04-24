using FluentValidation;
using HomeAssignment.DTOs.RequestDTOs.ChapterRelated;
using HomeworkAssignment.Application.Abstractions.ChapterRelated;
using HomeworkAssignment.AuthorizationFilters;
using HomeworkAssignment.Controllers.Abstractions;
using HomeworkAssignment.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Hybrid;

namespace HomeworkAssignment.Controllers.ChapterRelated;

/// <summary>
///     Controller for managing chapters within a course.
/// </summary>
[Produces("application/json")]
[Authorize]
[ApiController]
[Route("api/courses/{courseId:guid}/chapters")]
public class ChapterController(IChapterService service, HybridCache cache, ICacheKeyManager cacheKeyManager)
    : BaseController
{
    private readonly HybridCacheEntryOptions _cacheOptions = new()
    {
        LocalCacheExpiration = TimeSpan.FromMinutes(5),
        Expiration = TimeSpan.FromMinutes(10)
    };

    /// <summary>
    ///     Getting the all chapters of a course.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(Guid courseId, CancellationToken cancellationToken = default)
    {
        var cacheKey = cacheKeyManager.ChapterList(courseId);
        var cachedChapter = await cache.GetOrCreateAsync(
            cacheKey,
            async _ => await service.GetChaptersAsync(courseId, cancellationToken),
            _cacheOptions,
            [cacheKeyManager.CourseSingleGroup(courseId)],
            cancellationToken);

        return Ok(cachedChapter);
    }

    /// <summary>
    ///     Getting the first chapter of a course.
    /// </summary>
    [HttpGet("first")]
    public async Task<IActionResult> GetFirst(Guid courseId, CancellationToken cancellationToken = default)
    {
        var cacheKey = cacheKeyManager.ChapterFirst(courseId);
        var cachedChapter = await cache.GetOrCreateAsync(
            cacheKey,
            async _ => await service.GetFirstChapterByCourseIdAsync(courseId, cancellationToken),
            _cacheOptions,
            [cacheKeyManager.CourseSingleGroup(courseId)],
            cancellationToken);

        if (cachedChapter == null) return NotFound(courseId);
        return Ok(cachedChapter);
    }

    /// <summary>
    ///     Getting a single chapter by ID for a course.
    /// </summary>
    [HttpGet("{chapterId:guid}")]
    public async Task<IActionResult> Get(Guid courseId, Guid chapterId, CancellationToken cancellationToken = default)
    {
        var cacheKey = cacheKeyManager.ChapterSingle(courseId, chapterId);

        var cachedChapter = await cache.GetOrCreateAsync(
            cacheKey,
            async _ => await service.GetChapterByIdAsync(courseId, chapterId, cancellationToken),
            _cacheOptions,
            [cacheKeyManager.CourseSingleGroup(courseId), cacheKeyManager.ChapterSingleGroup(courseId, chapterId)],
            cancellationToken);

        if (cachedChapter == null) return NotFound(chapterId);
        return Ok(cachedChapter);
    }

    /// <summary>
    ///     Getting a next single chapter by ID for a request chapter.
    /// </summary>
    [HttpGet("{chapterId:guid}/next")]
    public async Task<IActionResult> GetNext(Guid courseId, Guid chapterId,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = cacheKeyManager.ChapterNext(courseId, chapterId);

        var hasNexChapter = await cache.GetOrCreateAsync(
            cacheKey,
            async _ => await service.GetNextChapterByChapterIdAsync(courseId, chapterId, cancellationToken),
            _cacheOptions,
            [cacheKeyManager.CourseSingleGroup(courseId), cacheKeyManager.ChapterSingleGroup(courseId, chapterId)],
            cancellationToken);

        return Ok(hasNexChapter);
    }

    /// <summary>
    ///     Creating a new chapter for a course.
    /// </summary>
    [HttpPost]
    [TeacherOnly]
    public async Task<IActionResult> Create(Guid courseId,
        [FromBody] RequestCreateChapterDto requestCreate,
        [FromServices] IValidator<RequestCreateChapterDto> validator,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await validator.ValidateAsync(requestCreate, cancellationToken);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var chapter = await service.CreateChapterAsync(courseId, requestCreate, cancellationToken);

        await cache.RemoveByTagAsync(cacheKeyManager.CourseSingleGroup(courseId), cancellationToken);
        return CreatedAtAction(nameof(Get), new { courseId, chapterId = chapter.Id }, chapter);
    }

    /// <summary>
    ///     Updating a chapter by ID for a course.
    /// </summary>
    [HttpPatch("{chapterId:guid}")]
    [TeacherOnly]
    public async Task<IActionResult> Update(Guid courseId, Guid chapterId,
        [FromBody] RequestPartialChapterDto request,
        [FromServices] IValidator<RequestPartialChapterDto> validator,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var chapter = await service.UpdateChapterAsync(courseId, chapterId, request, cancellationToken);

        await cache.RemoveByTagAsync(cacheKeyManager.CourseSingleGroup(courseId), cancellationToken);
        return Ok(chapter);
    }

    /// <summary>
    ///     Deleting a chapter by ID for a course.
    /// </summary>
    [HttpDelete("{chapterId:guid}")]
    [TeacherOnly]
    public async Task<IActionResult> Delete(Guid courseId, Guid chapterId,
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserId();
        await service.DeleteChapterAsync(userId, courseId, chapterId, cancellationToken);

        await cache.RemoveByTagAsync(cacheKeyManager.CourseSingleGroup(courseId), cancellationToken);
        return Ok(chapterId);
    }

    /// <summary>
    ///     Reordering chapters for a course.
    /// </summary>
    [HttpPut("reorder")]
    [TeacherOnly]
    public async Task<IActionResult> Reorder(Guid courseId,
        [FromBody] RequestReorderChapterDto[] chapterDtos,
        CancellationToken cancellationToken = default
    )
    {
        await service.ReorderChapterAsync(courseId, chapterDtos, cancellationToken);

        await cache.RemoveByTagAsync(cacheKeyManager.CourseSingleGroup(courseId), cancellationToken);
        return Ok(courseId);
    }

    /// <summary>
    ///     Publishing a chapter for a course.
    /// </summary>
    [HttpPatch("{chapterId:guid}/publish")]
    [TeacherOnly]
    public async Task<IActionResult> Publish(Guid courseId, Guid chapterId,
        CancellationToken cancellationToken = default)
    {
        await service.PublishChapterAsync(courseId, chapterId, cancellationToken);

        await cache.RemoveByTagAsync(cacheKeyManager.CourseSingleGroup(courseId), cancellationToken);
        return Ok(chapterId);
    }

    /// <summary>
    ///     Unpublishing a chapter for a course.
    /// </summary>
    [HttpPatch("{chapterId:guid}/unpublish")]
    [TeacherOnly]
    public async Task<IActionResult> Unpublish(Guid courseId, Guid chapterId,
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserId();
        await service.UnpublishChapterAsync(userId, courseId, chapterId, cancellationToken);

        await cache.RemoveByTagAsync(cacheKeyManager.CourseSingleGroup(courseId), cancellationToken);
        return Ok(chapterId);
    }
}