using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Attachments;

public sealed record UpdateAttachmentCommand(Guid Id, Attachment Attachment) : IRequest<Attachment>;