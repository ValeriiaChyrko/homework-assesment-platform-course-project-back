using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Chapters;

public record GetPublishedChapterByIdQuery(Guid Id, Guid OwnerId) : IRequest<Chapter?>;