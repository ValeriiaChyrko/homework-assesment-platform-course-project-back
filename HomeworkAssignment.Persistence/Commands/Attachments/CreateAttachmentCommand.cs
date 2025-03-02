using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Attachments;

public sealed record CreateAttachmentCommand(Attachment Attachment) : IRequest<Attachment>;