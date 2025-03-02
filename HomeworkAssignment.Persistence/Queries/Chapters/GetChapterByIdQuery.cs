using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Chapters;

public record GetChapterByIdQuery(Guid Id, Guid OwnerId) : IRequest<Chapter?>;