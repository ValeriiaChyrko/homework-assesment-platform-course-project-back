using HomeAssignment.Database.Contexts.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Attachments;

public sealed class DeleteAttachmentCommandHandler : IRequestHandler<DeleteAttachmentCommand>
{
    private readonly IHomeworkAssignmentDbContext _context;

    public DeleteAttachmentCommandHandler(IHomeworkAssignmentDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task Handle(DeleteAttachmentCommand command, CancellationToken cancellationToken)
    {
        if (command is null) throw new ArgumentNullException(nameof(command));

        var attachmentEntity = await _context.AttachmentEntities.FindAsync([command.Id], cancellationToken: cancellationToken);
        if (attachmentEntity != null) _context.AttachmentEntities.Remove(attachmentEntity);
    }
}