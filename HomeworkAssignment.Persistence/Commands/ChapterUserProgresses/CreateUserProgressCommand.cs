using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.ChapterUserProgresses;

public sealed record CreateUserProgressCommand(ChapterUserProgress ChapterUserProgress) : IRequest<ChapterUserProgress>;