using HomeAssignment.Domain.Abstractions;
using HomeAssignment.Domain.Abstractions.Enums;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RequestDTOs.UserRelated;
using HomeAssignment.Persistence.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Users;

public class GetAllUsersByRoleQuery : IRequest<PagedList<User>>
{
    public RequestUserFilterParameters FilterParameters { get; init; }
    public UserRoles UserRole { get; init; }

    public GetAllUsersByRoleQuery(RequestUserFilterParameters filterParameters, UserRoles userRole)
    {
        FilterParameters = filterParameters;
        UserRole = userRole;
    }
}