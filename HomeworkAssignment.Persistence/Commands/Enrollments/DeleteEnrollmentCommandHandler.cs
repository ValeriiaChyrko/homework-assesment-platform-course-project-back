using HomeAssignment.Database.Contexts.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Enrollments;

public sealed class DeleteEnrollmentCommandHandler : IRequestHandler<DeleteEnrollmentCommand>
{
    private readonly IHomeworkAssignmentDbContext _context;

    public DeleteEnrollmentCommandHandler(IHomeworkAssignmentDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task Handle(DeleteEnrollmentCommand command, CancellationToken cancellationToken)
    {
        if (command is null) throw new ArgumentNullException(nameof(command));

        var enrollmentEntity = await _context.EnrollmentEntities.FindAsync([command.Id], cancellationToken: cancellationToken);
        if (enrollmentEntity != null) _context.EnrollmentEntities.Remove(enrollmentEntity);
    }
}