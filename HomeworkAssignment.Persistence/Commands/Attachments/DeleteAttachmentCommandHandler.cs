using HomeAssignment.Database.Contexts.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Attachments;

public sealed class DeleteAttachmentCommandHandler(IHomeworkAssignmentDbContext context)
    : IRequestHandler<DeleteAttachmentCommand>
{
    private readonly IHomeworkAssignmentDbContext
        _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task Handle(DeleteAttachmentCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var attachmentEntity = await _context.AttachmentEntities.FindAsync([command.Id], cancellationToken);
        if (attachmentEntity != null) _context.AttachmentEntities.Remove(attachmentEntity);
    }
}