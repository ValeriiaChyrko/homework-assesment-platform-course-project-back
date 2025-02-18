using HomeAssignment.Domain.Abstractions.Enums;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.SharedDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Users;

public class GetAllUsersByRoleQuery : IRequest<PagedList<UserDto>>
{
    public RequestUserFilterParameters FilterParameters { get; init; }
    public UserRoles UserRole { get; init; }

    public GetAllUsersByRoleQuery(RequestUserFilterParameters filterParameters, UserRoles userRole)
    {
        FilterParameters = filterParameters;
        UserRole = userRole;
    }
}