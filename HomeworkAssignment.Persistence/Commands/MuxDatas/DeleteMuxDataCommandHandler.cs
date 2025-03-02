using HomeAssignment.Database.Contexts.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.MuxDatas;

public sealed class DeleteMuxDataCommandHandler : IRequestHandler<DeleteMuxDataCommand>
{
    private readonly IHomeworkAssignmentDbContext _context;

    public DeleteMuxDataCommandHandler(IHomeworkAssignmentDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task Handle(DeleteMuxDataCommand command, CancellationToken cancellationToken)
    {
        if (command is null) throw new ArgumentNullException(nameof(command));

        var muxDataEntity = await _context.MuxDataEntities.FindAsync([command.Id], cancellationToken: cancellationToken);
        if (muxDataEntity != null) _context.MuxDataEntities.Remove(muxDataEntity);
    }
}