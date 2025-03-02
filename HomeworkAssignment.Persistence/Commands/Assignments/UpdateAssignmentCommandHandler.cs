using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Assignments;

public sealed record UpdateAssignmentCommandHandler : IRequestHandler<UpdateAssignmentCommand, Assignment>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public UpdateAssignmentCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public Task<Assignment> Handle(UpdateAssignmentCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command is null) throw new ArgumentNullException(nameof(command));

        var assignmentEntity = _mapper.Map<AssignmentEntity>(command.Assignment);
        assignmentEntity.Id = command.Id;
        _context.AssignmentEntities.Update(assignmentEntity);

        return Task.FromResult(_mapper.Map<Assignment>(assignmentEntity));
    }
}