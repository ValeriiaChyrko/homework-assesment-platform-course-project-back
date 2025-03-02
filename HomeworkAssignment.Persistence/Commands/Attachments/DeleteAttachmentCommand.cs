using MediatR;

namespace HomeAssignment.Persistence.Commands.Attachments;

public sealed record DeleteAttachmentCommand(Guid Id) : IRequest;