using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Courses;

public record GetCourseByOwnerIdQuery(Guid CourseId, Guid OwnerId) : IRequest<Course?>;