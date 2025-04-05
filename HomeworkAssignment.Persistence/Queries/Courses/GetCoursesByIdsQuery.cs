using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Enrollments;

public record GetCoursesByIdsQuery(List<Guid> CourseIds) : IRequest<List<Course>?>;