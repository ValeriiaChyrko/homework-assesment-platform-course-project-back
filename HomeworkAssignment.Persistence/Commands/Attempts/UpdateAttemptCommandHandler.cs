using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Attempts;

public sealed record UpdateAttemptCommandHandler : IRequestHandler<UpdateAttemptCommand, Attempt>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public UpdateAttemptCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public Task<Attempt> Handle(UpdateAttemptCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command is null) throw new ArgumentNullException(nameof(command));

        var attemptEntity = _mapper.Map<AttemptEntity>(command.Attempt);
        attemptEntity.Id = command.Id;
        _context.AttemptEntities.Update(attemptEntity);

        return Task.FromResult(_mapper.Map<Attempt>(attemptEntity));
    }
}