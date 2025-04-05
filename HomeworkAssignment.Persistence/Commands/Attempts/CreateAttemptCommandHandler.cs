using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Attempts;

public sealed class CreateAttemptCommandHandler : IRequestHandler<CreateAttemptCommand, Attempt>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public CreateAttemptCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }


    public async Task<Attempt> Handle(CreateAttemptCommand command, CancellationToken cancellationToken)
    {
        if (command is null) throw new ArgumentNullException(nameof(command));

        var attemptEntity = _mapper.Map<AttemptEntity>(command.Attempt);
        var addedEntity = await _context.AttemptEntities.AddAsync(attemptEntity, cancellationToken);

        return _mapper.Map<Attempt>(addedEntity.Entity);
    }
}