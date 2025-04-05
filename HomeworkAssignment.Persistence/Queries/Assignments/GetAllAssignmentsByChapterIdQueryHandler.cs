using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Assignments;

public sealed class
    GetAllAssignmentsByIdQueryHandler : IRequestHandler<GetAllAssignmentsByChapterIdQuery, IEnumerable<Assignment>>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetAllAssignmentsByIdQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<Assignment>> Handle(GetAllAssignmentsByChapterIdQuery query,
        CancellationToken cancellationToken)
    {
        var assignmentEntities = await _context
            .AssignmentEntities
            .Where(a => a.ChapterId == query.ChapterId)
            .OrderBy(a => a.Position)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return assignmentEntities.Select(entityModel => _mapper.Map<Assignment>(entityModel)).ToList();
    }
}