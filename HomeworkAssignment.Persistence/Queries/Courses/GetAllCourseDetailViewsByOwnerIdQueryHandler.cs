using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.Persistence.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Courses;

public sealed class
    GetAllCourseDetailViewsByOwnerIdQueryHandler : IRequestHandler<GetAllCourseDetailViewsByOwnerIdQuery,
    PagedList<CourseDetailView>>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetAllCourseDetailViewsByOwnerIdQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<PagedList<CourseDetailView>> Handle(GetAllCourseDetailViewsByOwnerIdQuery query,
        CancellationToken cancellationToken)
    {
        var coursesQuery = _context.CourseEntities.Where(a => a.UserId == query.UserId).AsNoTracking();

        if (!string.IsNullOrEmpty(query.FilterParameters.Title))
            coursesQuery = coursesQuery.Where(a => a.Title.Contains(query.FilterParameters.Title));

        if (query.FilterParameters.IncludeCategory) coursesQuery = coursesQuery.Include(a => a.Category);

        coursesQuery = coursesQuery.OrderByDescending(a => a.CreatedAt);

        var assignmentDtos = coursesQuery.Select(entityModel => _mapper.Map<CourseDetailView>(entityModel));
        return await PagedList<CourseDetailView>.CreateAsync(assignmentDtos, query.FilterParameters.Page,
            query.FilterParameters.PageSize, cancellationToken);
    }
}