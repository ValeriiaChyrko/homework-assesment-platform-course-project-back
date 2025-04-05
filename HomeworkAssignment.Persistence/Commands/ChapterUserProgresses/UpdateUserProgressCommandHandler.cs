using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.ChapterUserProgresses;

public sealed record UpdateUserProgressCommandHandler : IRequestHandler<UpdateUserProgressCommand, ChapterUserProgress>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public UpdateUserProgressCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public Task<ChapterUserProgress> Handle(UpdateUserProgressCommand progressCommand,
        CancellationToken cancellationToken = default)
    {
        if (progressCommand is null) throw new ArgumentNullException(nameof(progressCommand));

        var userProgressEntity = _mapper.Map<UserChapterProgressEntity>(progressCommand.ChapterUserProgress);
        _context.UserChapterProgressEntities.Update(userProgressEntity);

        return Task.FromResult(_mapper.Map<ChapterUserProgress>(userProgressEntity));
    }
}