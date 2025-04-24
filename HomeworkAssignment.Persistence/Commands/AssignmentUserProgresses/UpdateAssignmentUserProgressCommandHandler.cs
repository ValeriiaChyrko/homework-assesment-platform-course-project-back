using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.AssignmentUserProgresses;

public sealed class
    UpdateAssignmentUserProgressCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    : IRequestHandler<UpdateAssignmentUserProgressCommand,
        AssignmentUserProgress>
{
    private readonly IHomeworkAssignmentDbContext
        _context = context ?? throw new ArgumentNullException(nameof(context));

    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public Task<AssignmentUserProgress> Handle(UpdateAssignmentUserProgressCommand progressCommand,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(progressCommand);

        var userProgressEntity = _mapper.Map<UserAssignmentProgressEntity>(progressCommand.AssignmentUserProgress);
        _context.UserAssignmentProgressEntities.Update(userProgressEntity);

        return Task.FromResult(_mapper.Map<AssignmentUserProgress>(userProgressEntity));
    }
}