using MediatR;

namespace HomeAssignment.Persistence.Commands.Teachers;

public sealed record DeleteTeacherCommand(Guid UserId) : IRequest;