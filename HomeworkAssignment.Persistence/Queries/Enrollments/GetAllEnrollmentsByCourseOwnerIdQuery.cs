using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Enrollments;

public record GetAllEnrollmentsByCourseOwnerIdQuery(Guid OwnerId) : IRequest<IEnumerable<Enrollment>>;