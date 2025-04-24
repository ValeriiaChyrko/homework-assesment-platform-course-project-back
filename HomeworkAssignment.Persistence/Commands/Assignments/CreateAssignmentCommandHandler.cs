using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Assignments;

public sealed class CreateAssignmentCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    : IRequestHandler<CreateAssignmentCommand, Assignment>
{
    private readonly IHomeworkAssignmentDbContext
        _context = context ?? throw new ArgumentNullException(nameof(context));

    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<Assignment> Handle(CreateAssignmentCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var assignmentEntity = _mapper.Map<AssignmentEntity>(command.Assignment);
        var addedEntity = await _context.AssignmentEntities.AddAsync(assignmentEntity, cancellationToken);

        return _mapper.Map<Assignment>(addedEntity.Entity);
    }
}