using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.MuxDatas;

public sealed class CreateMuxDataCommandHandler : IRequestHandler<CreateMuxDataCommand, MuxData>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public CreateMuxDataCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }


    public async Task<MuxData> Handle(CreateMuxDataCommand command, CancellationToken cancellationToken)
    {
        if (command is null) throw new ArgumentNullException(nameof(command));

        var muxDataEntity = _mapper.Map<MuxDataEntity>(command.MuxData);
        var addedEntity = await _context.MuxDataEntities.AddAsync(muxDataEntity, cancellationToken);

        return _mapper.Map<MuxData>(addedEntity.Entity);
    }
}