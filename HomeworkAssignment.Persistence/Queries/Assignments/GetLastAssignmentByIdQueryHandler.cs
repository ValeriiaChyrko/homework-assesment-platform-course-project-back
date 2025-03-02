using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Assignments;

public sealed class GetLastAssignmentByIdQueryHandler : IRequestHandler<GetLastAssignmentByIdQuery, Assignment?>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetLastAssignmentByIdQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Assignment?> Handle(GetLastAssignmentByIdQuery query, CancellationToken cancellationToken)
    {
        var assignment = await _context
            .AssignmentEntities
            .AsNoTracking()
            .Where(mr => mr.ChapterId == query.ChapterId) 
            .OrderByDescending(mr => mr.Position) 
            .FirstOrDefaultAsync(cancellationToken); 

        return assignment != null ? _mapper.Map<Assignment>(assignment) : null;
    }
}