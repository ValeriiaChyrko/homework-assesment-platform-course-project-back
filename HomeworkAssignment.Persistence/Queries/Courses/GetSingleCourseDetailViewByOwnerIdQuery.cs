using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RequestDTOs.CourseRelated;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Courses;

public class GetSingleCourseDetailViewByOwnerIdQuery(RequestCourseFilterParameters filterParameters, Guid userId, Guid courseId) : IRequest<CourseDetailView?>
{
    public RequestCourseFilterParameters FilterParameters { get; init; } = filterParameters;
    public Guid UserId { get; init; } = userId;
    public Guid CourseId { get; init; } = courseId;
}