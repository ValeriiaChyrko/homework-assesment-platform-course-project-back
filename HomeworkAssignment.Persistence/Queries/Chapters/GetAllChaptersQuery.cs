using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.SharedDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Chapters;

public class GetAllChaptersQuery : IRequest<PagedList<Chapter>>
{
    public RequestChapterFilterParameters FilterParameters { get; init; }

    public GetAllChaptersQuery(RequestChapterFilterParameters filterParameters)
    {
        FilterParameters = filterParameters;
    }
}