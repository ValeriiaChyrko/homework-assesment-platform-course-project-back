using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.SharedDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Assignments;

public sealed class
    GetAllAssignmentsQueryHandler : IRequestHandler<GetAllAssignmentsQuery, PagedList<Assignment>>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetAllAssignmentsQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<PagedList<Assignment>> Handle(GetAllAssignmentsQuery query,
        CancellationToken cancellationToken)
    {
        var assignmentsQuery = _context.AssignmentEntities.AsNoTracking();
        
        if (!string.IsNullOrEmpty(query.FilterParameters.Title))
        {
            assignmentsQuery = assignmentsQuery.Where(a => a.Title.Contains(query.FilterParameters.Title));
        }
        
        if (query.FilterParameters.ChapterId.HasValue)
        {
            assignmentsQuery = assignmentsQuery.Where(a => a.ChapterId == query.FilterParameters.ChapterId.Value);
        }
        
        if (query.FilterParameters.IsPublished.HasValue)
        {
            assignmentsQuery = assignmentsQuery.Where(a => a.IsPublished == query.FilterParameters.IsPublished.Value);
        }
        
        if (!string.IsNullOrEmpty(query.FilterParameters.SortBy))
        {
            assignmentsQuery = query.FilterParameters.IsAscending
                ? assignmentsQuery.OrderBy(a => EF.Property<object>(a, query.FilterParameters.SortBy))
                : assignmentsQuery.OrderByDescending(a => EF.Property<object>(a, query.FilterParameters.SortBy));
        }
        
        var assignmentDtos = assignmentsQuery.Select(entityModel => _mapper.Map<Assignment>(entityModel));
        return await PagedList<Assignment>.CreateAsync(assignmentDtos, query.FilterParameters.PageNumber, query.FilterParameters.PageSize, cancellationToken);
    }
}