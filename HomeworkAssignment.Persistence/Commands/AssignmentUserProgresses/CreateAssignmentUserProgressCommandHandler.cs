using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.AssignmentUserProgresses;

public sealed class CreateAssignmentUserProgressCommandHandler : IRequestHandler<CreateAssignmentUserProgressCommand, AssignmentUserProgress>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public CreateAssignmentUserProgressCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }


    public async Task<AssignmentUserProgress> Handle(CreateAssignmentUserProgressCommand progressCommand, CancellationToken cancellationToken)
    {
        if (progressCommand is null) throw new ArgumentNullException(nameof(progressCommand));

        var userProgressEntity = _mapper.Map<UserAssignmentProgressEntity>(progressCommand.AssignmentUserProgress);
        await _context.UserAssignmentProgressEntities.AddAsync(userProgressEntity, cancellationToken);

        return _mapper.Map<AssignmentUserProgress>(userProgressEntity);
    }
}