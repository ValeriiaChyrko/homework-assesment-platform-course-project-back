using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RequestDTOs.ChapterRelated;
using HomeAssignment.Persistence.Abstractions;
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