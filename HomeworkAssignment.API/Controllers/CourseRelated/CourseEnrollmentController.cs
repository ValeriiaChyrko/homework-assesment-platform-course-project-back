﻿using HomeworkAssignment.Application.Abstractions.CourseRelated;
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
[Route("api/courses/{courseId:guid}/enrollments")]
public class CourseEnrollmentController(
    ICourseEnrollmentService service,
    HybridCache cache,
    ICacheKeyManager cacheKeyManager) : BaseController
{
    private readonly HybridCacheEntryOptions _cacheOptions = new()
    {
        LocalCacheExpiration = TimeSpan.FromMinutes(2),
        Expiration = TimeSpan.FromMinutes(5)
    };

    /// <summary>
    ///     Gets all enrolled courses for a user.
    /// </summary>
    [HttpGet("/api/courses/enrollments/statistics")]
    [TeacherOnly]
    public async Task<IActionResult> GetEnrollmentsAnalytics(CancellationToken cancellationToken = default)
    {
        var userId = GetUserId();
        var cacheKey = cacheKeyManager.EnrollmentsAnalytics(userId);

        var cachedEnrollments = await cache.GetOrCreateAsync(
            cacheKey,
            async _ => await service.GetEnrollmentsAnalyticsAsync(userId, cancellationToken),
            _cacheOptions,
            [cacheKeyManager.EnrollmentListGroup(userId)],
            cancellationToken);

        return Ok(cachedEnrollments);
    }

    /// <summary>
    ///     Gets enrollment details of a user for a course.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetEnrollment(Guid courseId, CancellationToken cancellationToken = default)
    {
        var userId = GetUserId();
        var cacheKey = cacheKeyManager.Enrollment(userId, courseId);

        var cachedEnrollment = await cache.GetOrCreateAsync(
            cacheKey,
            async _ => await service.GetEnrollmentAsync(userId, courseId, cancellationToken),
            _cacheOptions,
            [cacheKeyManager.EnrollmentListGroup(userId)],
            cancellationToken);

        return Ok(cachedEnrollment);
    }

    /// <summary>
    ///     Enrolls a user in a course.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Enroll(Guid courseId, CancellationToken cancellationToken = default)
    {
        var userId = GetUserId();
        var result = await service.EnrollAsync(userId, courseId, cancellationToken);

        await cache.RemoveByTagAsync(cacheKeyManager.EnrollmentListGroup(userId), cancellationToken);
        await cache.RemoveByTagAsync(cacheKeyManager.CourseListGroup(userId), cancellationToken);
        await cache.RemoveByTagAsync(cacheKeyManager.CourseSingleGroup(courseId), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    ///     Withdraws a user from a course.
    /// </summary>
    [HttpPatch("withdraw")]
    public async Task<IActionResult> Withdraw(Guid courseId, CancellationToken cancellationToken = default)
    {
        var userId = GetUserId();
        await service.WithdrawAsync(userId, courseId, cancellationToken);

        await cache.RemoveByTagAsync(cacheKeyManager.EnrollmentListGroup(userId), cancellationToken);
        await cache.RemoveByTagAsync(cacheKeyManager.CourseListGroup(userId), cancellationToken);
        return Ok(courseId);
    }
}