using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Attachments;

public sealed record UpdateAttachmentCommandHandler : IRequestHandler<UpdateAttachmentCommand, Attachment>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public UpdateAttachmentCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public Task<Attachment> Handle(UpdateAttachmentCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command is null) throw new ArgumentNullException(nameof(command));
        
        var attachmentEntity = _mapper.Map<AttachmentEntity>(command.Attachment);
        attachmentEntity.Id = command.Id;
        _context.AttachmentEntities.Update(attachmentEntity);

        return Task.FromResult(_mapper.Map<Attachment>(attachmentEntity));
    }
}