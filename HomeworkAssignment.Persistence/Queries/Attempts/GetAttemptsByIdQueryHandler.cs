using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Attempts;

public sealed class GetAttemptsByIdQueryHandler : IRequestHandler<GetAttemptByIdQuery, Attempt?>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetAttemptsByIdQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Attempt?> Handle(GetAttemptByIdQuery query, CancellationToken cancellationToken)
    {
        var attempt = await _context
            .AttemptEntities
            .AsNoTracking()
            .SingleOrDefaultAsync(mr => 
                    mr.Id == query.AttemptId && mr.AssignmentId == query.AssignmentId,
                cancellationToken
            );

        return attempt != null ? _mapper.Map<Attempt>(attempt) : null;
    }
}