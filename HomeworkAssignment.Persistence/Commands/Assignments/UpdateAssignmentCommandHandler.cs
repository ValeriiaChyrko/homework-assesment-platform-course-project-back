using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Assignments;

public sealed class UpdateAssignmentCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    : IRequestHandler<UpdateAssignmentCommand, Assignment>
{
    private readonly IHomeworkAssignmentDbContext
        _context = context ?? throw new ArgumentNullException(nameof(context));

    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public Task<Assignment> Handle(UpdateAssignmentCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var assignmentEntity = _mapper.Map<AssignmentEntity>(command.Assignment);
        assignmentEntity.Id = command.Id;
        _context.AssignmentEntities.Update(assignmentEntity);

        return Task.FromResult(_mapper.Map<Assignment>(assignmentEntity));
    }
}