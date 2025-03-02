using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Attachments;

public record GetAllAttachmentsByChapterIdQuery(Guid ChapterId)
    : IRequest<IEnumerable<Attachment>>;