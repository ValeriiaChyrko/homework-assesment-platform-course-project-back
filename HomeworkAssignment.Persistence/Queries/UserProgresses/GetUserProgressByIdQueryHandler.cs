using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.UserProgresses;

public sealed class
    GetUserProgressByIdQueryHandler : IRequestHandler<GetUserProgressByIdQuery,
    UserProgress?>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetUserProgressByIdQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<UserProgress?> Handle(GetUserProgressByIdQuery query,
        CancellationToken cancellationToken)
    {
        var userProgressEntity = await _context
            .UserProgressEntities
            .AsNoTracking()
            .SingleOrDefaultAsync(
                a => a.ChapterId == query.ChapterId
                && a.UserId == query.UserId, 
                cancellationToken: cancellationToken
            );

        return userProgressEntity != null ? _mapper.Map<UserProgress>(userProgressEntity) : null;
    }
}