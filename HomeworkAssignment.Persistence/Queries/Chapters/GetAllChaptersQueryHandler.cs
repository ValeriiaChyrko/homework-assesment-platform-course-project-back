using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.Persistence.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Chapters;

public sealed class
    GetAllChaptersQueryHandler : IRequestHandler<GetAllChaptersQuery, PagedList<Chapter>>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetAllChaptersQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<PagedList<Chapter>> Handle(GetAllChaptersQuery query,
        CancellationToken cancellationToken)
    {
        var chaptersQuery = _context.ChapterEntities.AsNoTracking();

        if (!string.IsNullOrEmpty(query.FilterParameters.Title))
            chaptersQuery = chaptersQuery.Where(a => a.Title.Contains(query.FilterParameters.Title));

        if (query.FilterParameters.CourseId.HasValue)
            chaptersQuery = chaptersQuery.Where(a => a.CourseId == query.FilterParameters.CourseId.Value);

        if (query.FilterParameters.IsPublished.HasValue)
            chaptersQuery = chaptersQuery.Where(a => a.IsPublished == query.FilterParameters.IsPublished.Value);

        if (!string.IsNullOrEmpty(query.FilterParameters.SortBy))
            chaptersQuery = query.FilterParameters.IsAscending
                ? chaptersQuery.OrderBy(a => EF.Property<object>(a, query.FilterParameters.SortBy))
                : chaptersQuery.OrderByDescending(a => EF.Property<object>(a, query.FilterParameters.SortBy));

        var assignmentDtos = chaptersQuery.Select(entityModel => _mapper.Map<Chapter>(entityModel));
        return await PagedList<Chapter>.CreateAsync(assignmentDtos, query.FilterParameters.PageNumber,
            query.FilterParameters.PageSize, cancellationToken);
    }
}