using MediatR;

namespace HomeAssignment.Persistence.Commands.ChapterUserProgresses;

public sealed record DeleteUserProgressCommand(Guid Id) : IRequest;