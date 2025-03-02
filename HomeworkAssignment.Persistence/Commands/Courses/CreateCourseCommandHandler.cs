using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Courses;

public sealed class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, Course>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public CreateCourseCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }


    public async Task<Course> Handle(CreateCourseCommand command, CancellationToken cancellationToken)
    {
        if (command is null) throw new ArgumentNullException(nameof(command));

        var courseEntity = _mapper.Map<CourseEntity>(command.Course);
        var addedEntity = await _context.CourseEntities.AddAsync(courseEntity, cancellationToken);

        return _mapper.Map<Course>(addedEntity.Entity);
    }
}