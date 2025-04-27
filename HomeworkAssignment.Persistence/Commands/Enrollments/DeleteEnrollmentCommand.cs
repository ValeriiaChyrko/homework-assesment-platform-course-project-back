using MediatR;

namespace HomeAssignment.Persistence.Commands.Enrollments;

public sealed record DeleteEnrollmentCommand(Guid Id) : IRequest;