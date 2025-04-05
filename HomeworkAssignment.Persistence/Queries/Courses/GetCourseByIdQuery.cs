using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Courses;

public record GetCourseByIdQuery(Guid CourseId) : IRequest<Course?>;