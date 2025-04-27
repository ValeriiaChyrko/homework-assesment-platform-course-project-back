using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.Persistence.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Courses;

public sealed class GetAllCourseDetailViewsQueryHandler(
    IHomeworkAssignmentDbContext context,
    IMapper mapper)
    : IRequestHandler<GetAllCourseDetailViewsQuery, PagedList<CourseDetailView>>
{
    public async Task<PagedList<CourseDetailView>> Handle(GetAllCourseDetailViewsQuery query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var filter = query.FilterParameters;
        var baseQuery = context.CourseEntities.AsNoTracking();

        if (filter.FilterByStudent)
            baseQuery = from course in baseQuery
                join enrollment in context.EnrollmentEntities
                    on course.Id equals enrollment.CourseId
                where enrollment.UserId == query.UserId
                select course;

        if (!string.IsNullOrWhiteSpace(filter.Title))
            baseQuery = baseQuery.Where(course => EF.Functions.ILike(course.Title, $"%{filter.Title}%"));

        if (filter.CategoryId.HasValue)
            baseQuery = baseQuery.Where(course => course.CategoryId == filter.CategoryId.Value);

        if (filter.IsPublished) baseQuery = baseQuery.Where(course => course.IsPublished);

        if (filter.IncludeCategory) baseQuery = baseQuery.Include(course => course.Category);

        if (filter.IncludeChapters)
            baseQuery = baseQuery.Include(course => course.Chapters!
                .Where(chapter => chapter.IsPublished));

        baseQuery = baseQuery.OrderByDescending(course => course.CreatedAt);

        var projectedQuery = baseQuery.Select(course => mapper.Map<CourseDetailView>(course));

        return await PagedList<CourseDetailView>.CreateAsync(
            projectedQuery,
            filter.Page,
            filter.PageSize,
            cancellationToken);
    }
}