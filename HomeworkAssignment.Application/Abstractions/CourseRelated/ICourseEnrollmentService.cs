﻿using HomeAssignment.DTOs.RespondDTOs.CourseRelated;

namespace HomeworkAssignment.Application.Abstractions.CourseRelated;

public interface ICourseEnrollmentService
{
    Task<RespondEnrollmentDto> EnrollAsync(Guid userId, Guid courseId,
        CancellationToken cancellationToken = default);

    Task WithdrawAsync(Guid userId, Guid courseId, CancellationToken cancellationToken = default);

    Task<RespondEnrollmentDto?> GetEnrollmentAsync(Guid userId, Guid courseId,
        CancellationToken cancellationToken = default);

    Task<RespondEnrollmentsAnalyticsDto> GetEnrollmentsAnalyticsAsync(Guid userId,
        CancellationToken cancellationToken = default);
}