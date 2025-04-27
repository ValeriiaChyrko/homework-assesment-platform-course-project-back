using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Enrollments;

public sealed record CreateEnrollmentCommand(Enrollment Enrollment) : IRequest<Enrollment>;