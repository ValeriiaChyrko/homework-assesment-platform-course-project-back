using HomeAssignment.Domain.Abstractions.Enums;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Users;

public record CheckIfUserInRoleQuery(Guid UserId, UserRoles UserRole)
    : IRequest<bool>;