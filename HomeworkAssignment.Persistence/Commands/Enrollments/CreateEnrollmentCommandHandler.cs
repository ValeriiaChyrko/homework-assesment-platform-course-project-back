using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Enrollments;

public sealed class CreateEnrollmentCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    : IRequestHandler<CreateEnrollmentCommand, Enrollment>
{
    private readonly IHomeworkAssignmentDbContext
        _context = context ?? throw new ArgumentNullException(nameof(context));

    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));


    public async Task<Enrollment> Handle(CreateEnrollmentCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var enrollmentEntity = _mapper.Map<EnrollmentEntity>(command.Enrollment);
        var addedEntity = await _context.EnrollmentEntities.AddAsync(enrollmentEntity, cancellationToken);

        return _mapper.Map<Enrollment>(addedEntity.Entity);
    }
}