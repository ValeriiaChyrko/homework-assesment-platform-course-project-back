using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Enrollments;

public record GetAllEnrollmentsByUserIdQuery(Guid UserId)
    : IRequest<IEnumerable<Enrollment>>;