using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Enrollments;

public sealed record UpdateEnrollmentCommand(Guid Id, Enrollment Enrollment) : IRequest<Enrollment>;