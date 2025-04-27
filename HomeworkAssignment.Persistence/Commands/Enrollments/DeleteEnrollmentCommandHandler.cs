using HomeAssignment.Database.Contexts.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Enrollments;

public sealed class DeleteEnrollmentCommandHandler(IHomeworkAssignmentDbContext context)
    : IRequestHandler<DeleteEnrollmentCommand>
{
    private readonly IHomeworkAssignmentDbContext
        _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task Handle(DeleteEnrollmentCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var enrollmentEntity = await _context.EnrollmentEntities.FindAsync([command.Id], cancellationToken);
        if (enrollmentEntity != null) _context.EnrollmentEntities.Remove(enrollmentEntity);
    }
}