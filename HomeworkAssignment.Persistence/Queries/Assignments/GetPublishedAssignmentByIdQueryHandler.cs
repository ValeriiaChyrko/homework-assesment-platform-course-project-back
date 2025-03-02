using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Assignments;

public sealed class GetPublishedAssignmentByIdQueryHandler : IRequestHandler<GetPublishedAssignmentByIdQuery, Assignment?>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetPublishedAssignmentByIdQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Assignment?> Handle(GetPublishedAssignmentByIdQuery query, CancellationToken cancellationToken)
    {
        var assignment = await _context
            .AssignmentEntities
            .AsNoTracking()
            .SingleOrDefaultAsync(mr => 
                mr.Id == query.Id 
                && mr.ChapterId == query.ChapterId
                && mr.IsPublished == true, 
                cancellationToken
            );

        return assignment != null ? _mapper.Map<Assignment>(assignment) : null;
    }
}