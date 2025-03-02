using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Courses;

public record GetPublishedCourseByIdQuery(Guid Id, Guid OwnerId) : IRequest<Course?>;