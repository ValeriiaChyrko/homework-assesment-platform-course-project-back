﻿using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Users;

public sealed record UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, User>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public Task<User> Handle(UpdateUserCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command is null) throw new ArgumentNullException(nameof(command));

        var userEntity = _mapper.Map<UserEntity>(command.User);
        _context.UserEntities.Update(userEntity);

        return Task.FromResult(_mapper.Map<User>(userEntity));
    }
}