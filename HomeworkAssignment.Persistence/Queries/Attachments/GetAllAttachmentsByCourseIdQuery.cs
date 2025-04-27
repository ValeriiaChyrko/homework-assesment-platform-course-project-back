using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Attachments;

public record GetAllAttachmentsByCourseIdQuery(Guid CourseId)
    : IRequest<IEnumerable<Attachment>>;