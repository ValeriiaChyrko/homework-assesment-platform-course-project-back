using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.SharedDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Assignments;

public class GetAllAssignmentsQuery : IRequest<PagedList<Assignment>>
{
    public RequestAssignmentFilterParameters FilterParameters { get; init; }

    public GetAllAssignmentsQuery(RequestAssignmentFilterParameters filterParameters)
    {
        FilterParameters = filterParameters;
    }
}