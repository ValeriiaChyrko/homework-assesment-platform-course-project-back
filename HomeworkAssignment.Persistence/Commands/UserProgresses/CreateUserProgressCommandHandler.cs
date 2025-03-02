using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.UserProgresses;

public sealed class CreateUserProgressCommandHandler : IRequestHandler<CreateUserProgressCommand, UserProgress>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public CreateUserProgressCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }


    public async Task<UserProgress> Handle(CreateUserProgressCommand progressCommand, CancellationToken cancellationToken)
    {
        if (progressCommand is null) throw new ArgumentNullException(nameof(progressCommand));

        var userProgressEntity = _mapper.Map<UserProgressEntity>(progressCommand.UserProgress);
        await _context.UserProgressEntities.AddAsync(userProgressEntity, cancellationToken);

        return _mapper.Map<UserProgress>(userProgressEntity);
    }
}