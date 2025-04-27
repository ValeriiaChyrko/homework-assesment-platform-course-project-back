using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.Persistence.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Courses;

public sealed class GetAllCourseDetailViewsByOwnerIdQueryHandler(
    IHomeworkAssignmentDbContext context,
    IMapper mapper)
    : IRequestHandler<GetAllCourseDetailViewsByOwnerIdQuery, PagedList<CourseDetailView>>
{
    public async Task<PagedList<CourseDetailView>> Handle(GetAllCourseDetailViewsByOwnerIdQuery query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var filter = query.FilterParameters;
        var baseQuery = context.CourseEntities
            .AsNoTracking()
            .Where(c => c.UserId == query.UserId);

        if (!string.IsNullOrWhiteSpace(filter.Title))
            baseQuery = baseQuery.Where(c => EF.Functions.ILike(c.Title, $"%{filter.Title}%"));

        if (filter.IncludeCategory) baseQuery = baseQuery.Include(c => c.Category);

        var orderedQuery = baseQuery.OrderByDescending(c => c.CreatedAt);

        var projectedQuery = orderedQuery.Select(c => mapper.Map<CourseDetailView>(c));

        return await PagedList<CourseDetailView>.CreateAsync(
            projectedQuery,
            filter.Page,
            filter.PageSize,
            cancellationToken);
    }
}