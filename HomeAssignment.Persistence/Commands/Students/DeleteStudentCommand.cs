using MediatR;

namespace HomeAssignment.Persistence.Commands.Students;

public sealed record DeleteStudentCommand(Guid UserId) : IRequest;