using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.ChapterUserProgresses;

public sealed record UpdateUserProgressCommand(ChapterUserProgress ChapterUserProgress) : IRequest<ChapterUserProgress>;