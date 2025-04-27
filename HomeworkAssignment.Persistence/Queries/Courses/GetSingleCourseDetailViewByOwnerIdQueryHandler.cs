using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Courses;

public sealed class GetSingleCourseDetailViewByOwnerIdQueryHandler(
    IHomeworkAssignmentDbContext context,
    IMapper mapper)
    : IRequestHandler<GetSingleCourseDetailViewByOwnerIdQuery, CourseDetailView?>
{
    public async Task<CourseDetailView?> Handle(GetSingleCourseDetailViewByOwnerIdQuery query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var queryable = context.CourseEntities
            .AsNoTracking()
            .Where(course => course.UserId == query.UserId && course.Id == query.CourseId);

        var filters = query.FilterParameters;

        if (filters.IncludeCategory)
            queryable = queryable.Include(course => course.Category);

        if (filters.IncludeChapters)
            queryable = queryable.Include(course => course.Chapters);

        if (filters.IncludeAttachments)
            queryable = queryable.Include(course => course.Attachments);

        var courseEntity = await queryable.SingleOrDefaultAsync(cancellationToken);

        return courseEntity is null
            ? null
            : mapper.Map<CourseDetailView>(courseEntity);
    }
}