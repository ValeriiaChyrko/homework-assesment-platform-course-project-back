using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Attempts;

public sealed class GetLastAttemptByIdQueryHandler : IRequestHandler<GetLastAttemptByIdQuery, Attempt?>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetLastAttemptByIdQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Attempt?> Handle(GetLastAttemptByIdQuery query, CancellationToken cancellationToken)
    {
        var chapter = await _context
            .AttemptEntities
            .AsNoTracking()
            .Where(mr => mr.UserId == query.UserId && mr.AssignmentId == query.AssignmentId)
            .OrderByDescending(mr => mr.Position)
            .FirstOrDefaultAsync(cancellationToken);

        return chapter != null ? _mapper.Map<Attempt>(chapter) : null;
    }
}