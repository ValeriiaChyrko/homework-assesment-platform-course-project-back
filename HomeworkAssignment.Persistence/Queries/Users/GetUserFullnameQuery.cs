using MediatR;

namespace HomeAssignment.Persistence.Queries.Users;

public record GetUserFullnameQuery(Guid UserId) : IRequest<string?>;