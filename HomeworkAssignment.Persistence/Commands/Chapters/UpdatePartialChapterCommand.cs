using MediatR;

namespace HomeAssignment.Persistence.Commands.Chapters;

public sealed record UpdatePartialChapterCommand(Guid ChapterId, int Position) : IRequest;