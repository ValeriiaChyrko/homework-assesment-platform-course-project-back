using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RequestDTOs.ChapterRelated;
using HomeAssignment.Persistence.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Chapters;

public class GetAllChaptersQuery : IRequest<PagedList<Chapter>>
{
    public GetAllChaptersQuery(RequestChapterFilterParameters filterParameters)
    {
        FilterParameters = filterParameters;
    }

    public RequestChapterFilterParameters FilterParameters { get; init; }
}