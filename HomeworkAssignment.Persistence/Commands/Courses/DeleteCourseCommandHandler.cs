using HomeAssignment.Database.Contexts.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Courses;

public sealed class DeleteCourseCommandHandler(IHomeworkAssignmentDbContext context)
    : IRequestHandler<DeleteCourseCommand>
{
    private readonly IHomeworkAssignmentDbContext
        _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task Handle(DeleteCourseCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var courseEntity = await _context.CourseEntities.FindAsync([command.Id], cancellationToken);
        if (courseEntity != null) _context.CourseEntities.Remove(courseEntity);
    }
}