using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Users;

public sealed class CreateUserCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    : IRequestHandler<CreateUserCommand, User>
{
    private readonly IHomeworkAssignmentDbContext
        _context = context ?? throw new ArgumentNullException(nameof(context));

    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));


    public async Task<User> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var userEntity = _mapper.Map<UserEntity>(command.User);
        await _context.UserEntities.AddAsync(userEntity, cancellationToken);

        return _mapper.Map<User>(userEntity);
    }
}