using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Chapters;

public sealed record UpdateChapterCommand(Guid Id, Chapter Chapter)
    : IRequest<Chapter>;