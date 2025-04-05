using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.UserAssignmentProgresses;

public sealed class
    GetUserProgressByIdQueryHandler : IRequestHandler<GetAssignmentUserProgressByIdQuery,
    AssignmentUserProgress?>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetUserProgressByIdQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<AssignmentUserProgress?> Handle(GetAssignmentUserProgressByIdQuery query,
        CancellationToken cancellationToken)
    {
        var userProgressEntity = await _context
            .UserAssignmentProgressEntities
            .AsNoTracking()
            .SingleOrDefaultAsync(
                a => a.AssignmentId == query.AssignmentId
                && a.UserId == query.UserId, 
                cancellationToken: cancellationToken
            );

        return userProgressEntity != null ? _mapper.Map<AssignmentUserProgress>(userProgressEntity) : null;
    }
}