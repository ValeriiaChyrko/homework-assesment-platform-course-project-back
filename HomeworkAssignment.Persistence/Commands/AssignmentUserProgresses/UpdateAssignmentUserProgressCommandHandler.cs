using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.AssignmentUserProgresses;

public sealed record
    UpdateAssignmentUserProgressCommandHandler : IRequestHandler<UpdateAssignmentUserProgressCommand,
    AssignmentUserProgress>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public UpdateAssignmentUserProgressCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public Task<AssignmentUserProgress> Handle(UpdateAssignmentUserProgressCommand progressCommand,
        CancellationToken cancellationToken = default)
    {
        if (progressCommand is null) throw new ArgumentNullException(nameof(progressCommand));

        var userProgressEntity = _mapper.Map<UserAssignmentProgressEntity>(progressCommand.AssignmentUserProgress);
        _context.UserAssignmentProgressEntities.Update(userProgressEntity);

        return Task.FromResult(_mapper.Map<AssignmentUserProgress>(userProgressEntity));
    }
}