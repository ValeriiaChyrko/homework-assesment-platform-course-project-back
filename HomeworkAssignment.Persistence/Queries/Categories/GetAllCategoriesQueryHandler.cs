using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Categories;

public sealed class GetAllCategoriesQueryHandler(
    IHomeworkAssignmentDbContext context,
    IMapper mapper)
    : IRequestHandler<GetAllCategoriesQuery, IEnumerable<Category>>
{
    public async Task<IEnumerable<Category>> Handle(GetAllCategoriesQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var categoryDtos = await context.CategoryEntities
            .OrderBy(a => a.Name)
            .AsNoTracking()
            .Select(a => mapper.Map<Category>(a))
            .ToListAsync(cancellationToken);

        return categoryDtos;
    }
}