using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Categories;

public sealed class
    GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery,
    IEnumerable<Category>>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetAllCategoriesQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<Category>> Handle(GetAllCategoriesQuery query,
        CancellationToken cancellationToken)
    {
        var categoryEntities = await _context
            .CategoryEntities
            .OrderBy(a => a.Name)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return categoryEntities.Select(entityModel => _mapper.Map<Category>(entityModel)).ToList();
    }
}