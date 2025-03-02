using MediatR;

namespace HomeAssignment.Persistence.Commands.Chapters;

public sealed record DeleteChapterCommand(Guid Id) : IRequest;