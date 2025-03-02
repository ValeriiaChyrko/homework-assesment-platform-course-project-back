using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Enrollments;

public record GetEnrollmentByIdQuery(Guid UserId, Guid CourseId) : IRequest<Enrollment?>;