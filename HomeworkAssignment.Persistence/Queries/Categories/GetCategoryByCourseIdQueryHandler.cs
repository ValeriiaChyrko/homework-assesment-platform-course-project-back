using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Categories;

public sealed class GetCategoryByCourseIdQueryHandler(
    IHomeworkAssignmentDbContext context,
    IMapper mapper)
    : IRequestHandler<GetCategoryByCourseIdQuery, Category?>
{
    public async Task<Category?> Handle(GetCategoryByCourseIdQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var categoryEntity = await context.CategoryEntities
            .AsNoTracking()
            .Where(c => c.Courses != null && c.Courses.Any(course => course.Id == query.CourseId))
            .FirstOrDefaultAsync(cancellationToken);

        return categoryEntity != null ? mapper.Map<Category>(categoryEntity) : null;
    }
}