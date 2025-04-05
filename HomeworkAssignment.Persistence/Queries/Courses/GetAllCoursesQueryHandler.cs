using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.SharedDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Courses;

public sealed class
    GetAllCoursesQueryHandler : IRequestHandler<GetAllCoursesQuery, PagedList<Course>>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetAllCoursesQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<PagedList<Course>> Handle(GetAllCoursesQuery query,
        CancellationToken cancellationToken)
    {
        var coursesQuery = _context.CourseEntities.AsNoTracking();
        
        if (!string.IsNullOrEmpty(query.FilterParameters.Title))
        {
            coursesQuery = coursesQuery.Where(a => a.Title.Contains(query.FilterParameters.Title));
        }
        
        if (query.FilterParameters.OwnerId.HasValue)
        {
            coursesQuery = coursesQuery.Where(a => a.UserId == query.FilterParameters.OwnerId.Value);
        }
        
        if (query.FilterParameters.CategoryId.HasValue)
        {
            coursesQuery = coursesQuery.Where(a => a.CategoryId == query.FilterParameters.CategoryId.Value);
        }
        
        if (query.FilterParameters.IsPublished.HasValue)
        {
            coursesQuery = coursesQuery.Where(a => a.IsPublished == query.FilterParameters.IsPublished.Value);
        }
        
        if (!string.IsNullOrEmpty(query.FilterParameters.SortBy))
        {
            coursesQuery = query.FilterParameters.IsAscending
                ? coursesQuery.OrderBy(a => EF.Property<object>(a, query.FilterParameters.SortBy))
                : coursesQuery.OrderByDescending(a => EF.Property<object>(a, query.FilterParameters.SortBy));
        }
        
        var assignmentDtos = coursesQuery.Select(entityModel => _mapper.Map<Course>(entityModel));
        return await PagedList<Course>.CreateAsync(assignmentDtos, query.FilterParameters.PageNumber, query.FilterParameters.PageSize, cancellationToken);
    }
}