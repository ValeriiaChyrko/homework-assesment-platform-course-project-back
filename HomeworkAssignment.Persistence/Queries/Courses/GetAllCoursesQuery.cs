using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RequestDTOs.CourseRelated;
using HomeAssignment.Persistence.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Courses;

public class GetAllCoursesQuery(RequestCourseFilterParameters filterParameters, Guid userId) : IRequest<PagedList<CourseDetailView>>
{
    public RequestCourseFilterParameters FilterParameters { get; init; } = filterParameters;
    public Guid UserId { get; init; }
}