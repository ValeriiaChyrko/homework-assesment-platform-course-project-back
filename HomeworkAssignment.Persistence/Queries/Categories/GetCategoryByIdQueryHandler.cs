using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Categories;

public sealed class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Category?>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetCategoryByIdQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Category?> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
    {
        var assignment = await _context
            .CategoryEntities
            .AsNoTracking()
            .SingleOrDefaultAsync(mr =>
                    mr.Id == query.Id,
                cancellationToken
            );

        return assignment != null ? _mapper.Map<Category>(assignment) : null;
    }
}