using HomeAssignment.Database.Contexts.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Courses;

public sealed class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand>
{
    private readonly IHomeworkAssignmentDbContext _context;

    public DeleteCourseCommandHandler(IHomeworkAssignmentDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task Handle(DeleteCourseCommand command, CancellationToken cancellationToken)
    {
        if (command is null) throw new ArgumentNullException(nameof(command));

        var courseEntity = await _context.CourseEntities.FindAsync([command.Id], cancellationToken);
        if (courseEntity != null) _context.CourseEntities.Remove(courseEntity);
    }
}