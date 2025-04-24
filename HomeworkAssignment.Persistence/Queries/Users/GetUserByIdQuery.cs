using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Users;

public record GetUserByIdQuery(Guid UserId) : IRequest<User?>;