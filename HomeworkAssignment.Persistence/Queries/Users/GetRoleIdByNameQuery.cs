using MediatR;

namespace HomeAssignment.Persistence.Queries.Users;

public record GetRoleIdByNameQuery(string RoleName) : IRequest<int>;