using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.MuxDatas;

public sealed class
    GetMuxDataByChapterIdQueryHandler : IRequestHandler<GetMuxDataByChapterIdQuery,
    MuxData?>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetMuxDataByChapterIdQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<MuxData?> Handle(GetMuxDataByChapterIdQuery query,
        CancellationToken cancellationToken)
    {
        var muxDataEntity = await _context
            .MuxDataEntities
            .AsNoTracking()
            .SingleOrDefaultAsync(
                a => a.ChapterId == query.ChapterId, 
                cancellationToken: cancellationToken
            );

        return muxDataEntity != null ? _mapper.Map<MuxData>(muxDataEntity) : null;
    }
}