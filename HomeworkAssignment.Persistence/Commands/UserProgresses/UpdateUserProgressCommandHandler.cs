using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.UserProgresses;

public sealed record UpdateUserProgressCommandHandler : IRequestHandler<UpdateUserProgressCommand, UserProgress>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public UpdateUserProgressCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public Task<UserProgress> Handle(UpdateUserProgressCommand progressCommand,
        CancellationToken cancellationToken = default)
    {
        if (progressCommand is null) throw new ArgumentNullException(nameof(progressCommand));

        var userProgressEntity = _mapper.Map<UserProgressEntity>(progressCommand.UserProgress);
        _context.UserProgressEntities.Update(userProgressEntity);

        return Task.FromResult(_mapper.Map<UserProgress>(userProgressEntity));
    }
}