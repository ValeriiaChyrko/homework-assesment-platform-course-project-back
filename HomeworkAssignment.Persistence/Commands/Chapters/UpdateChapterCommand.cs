using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Chapters;

public sealed record UpdateChapterCommand(Guid ChapterId, Chapter Chapter)
    : IRequest<Chapter>;