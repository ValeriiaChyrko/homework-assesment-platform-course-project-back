using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Attempts;

public sealed class UpdateAttemptCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    : IRequestHandler<UpdateAttemptCommand, Attempt>
{
    private readonly IHomeworkAssignmentDbContext
        _context = context ?? throw new ArgumentNullException(nameof(context));

    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public Task<Attempt> Handle(UpdateAttemptCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var attemptEntity = _mapper.Map<AttemptEntity>(command.Attempt);
        attemptEntity.Id = command.Id;
        _context.AttemptEntities.Update(attemptEntity);

        return Task.FromResult(_mapper.Map<Attempt>(attemptEntity));
    }
}