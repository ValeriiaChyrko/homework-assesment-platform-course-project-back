using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Courses;

public sealed class GetSingleCourseDetailViewByStudentIdQueryHandler(
    IHomeworkAssignmentDbContext context,
    IMapper mapper)
    : IRequestHandler<GetSingleCourseDetailViewByStudentIdQuery, CourseDetailView?>
{
    public async Task<CourseDetailView?> Handle(
        GetSingleCourseDetailViewByStudentIdQuery query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var filters = query.FilterParameters;

        var queryable = context.CourseEntities
            .AsNoTracking()
            .Where(c => c.Id == query.CourseId && c.IsPublished);

        if (filters.IncludeCategory)
            queryable = queryable.Include(c => c.Category);

        if (filters.IncludeChapters)
            queryable = queryable.Include(c => c.Chapters!.Where(ch => ch.IsPublished));

        if (filters.IncludeAttachments)
            queryable = queryable.Include(c => c.Attachments);

        var courseEntity = await queryable.SingleOrDefaultAsync(cancellationToken);

        return courseEntity is null
            ? null
            : mapper.Map<CourseDetailView>(courseEntity);
    }
}