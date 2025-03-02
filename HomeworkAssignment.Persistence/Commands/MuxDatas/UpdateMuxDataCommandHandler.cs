using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.MuxDatas;

public sealed record UpdateMuxDataCommandHandler : IRequestHandler<UpdateMuxDataCommand, MuxData>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public UpdateMuxDataCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public Task<MuxData> Handle(UpdateMuxDataCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command is null) throw new ArgumentNullException(nameof(command));
        
        var muxDataEntity = _mapper.Map<MuxDataEntity>(command.MuxData);
        muxDataEntity.Id = command.Id;
        _context.MuxDataEntities.Update(muxDataEntity);

        return Task.FromResult(_mapper.Map<MuxData>(muxDataEntity));
    }
}