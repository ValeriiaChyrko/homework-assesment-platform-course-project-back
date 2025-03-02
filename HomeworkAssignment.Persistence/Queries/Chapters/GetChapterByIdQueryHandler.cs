using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Chapters;

public sealed class GetChapterByIdQueryHandler : IRequestHandler<GetChapterByIdQuery, Chapter?>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetChapterByIdQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Chapter?> Handle(GetChapterByIdQuery query, CancellationToken cancellationToken)
    {
        var assignment = await _context
            .CourseEntities
            .AsNoTracking()
            .SingleOrDefaultAsync(mr => 
                mr.Id == query.Id && mr.UserId == query.OwnerId, 
                cancellationToken
            );

        return assignment != null ? _mapper.Map<Chapter>(assignment) : null;
    }
}