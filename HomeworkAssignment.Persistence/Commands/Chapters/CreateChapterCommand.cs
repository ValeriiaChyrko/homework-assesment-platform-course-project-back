using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Chapters;

public sealed record CreateChapterCommand(Chapter Chapter) : IRequest<Chapter>;