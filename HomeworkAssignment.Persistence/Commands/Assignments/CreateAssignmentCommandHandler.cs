using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Assignments;

public sealed class CreateAssignmentCommandHandler : IRequestHandler<CreateAssignmentCommand, Assignment>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public CreateAssignmentCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }


    public async Task<Assignment> Handle(CreateAssignmentCommand command, CancellationToken cancellationToken)
    {
        if (command is null) throw new ArgumentNullException(nameof(command));

        var assignmentEntity = _mapper.Map<AssignmentEntity>(command.Assignment);
        var addedEntity = await _context.AssignmentEntities.AddAsync(assignmentEntity, cancellationToken);

        return _mapper.Map<Assignment>(addedEntity.Entity);
    }
}