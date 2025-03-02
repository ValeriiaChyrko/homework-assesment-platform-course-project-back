using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Attachments;

public sealed class CreateAttachmentCommandHandler : IRequestHandler<CreateAttachmentCommand, Attachment>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public CreateAttachmentCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }


    public async Task<Attachment> Handle(CreateAttachmentCommand command, CancellationToken cancellationToken)
    {
        if (command is null) throw new ArgumentNullException(nameof(command));

        var attachmentEntity = _mapper.Map<AttachmentEntity>(command.Attachment);
        var addedEntity = await _context.AttachmentEntities.AddAsync(attachmentEntity, cancellationToken);

        return _mapper.Map<Attachment>(addedEntity.Entity);
    }
}