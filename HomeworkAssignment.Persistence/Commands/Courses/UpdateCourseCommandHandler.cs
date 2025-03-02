using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Courses;

public sealed record UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand, Course>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public UpdateCourseCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public Task<Course> Handle(UpdateCourseCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command is null) throw new ArgumentNullException(nameof(command));

        var courseEntity = _mapper.Map<CourseEntity>(command.Course);
        courseEntity.Id = command.Id;
        _context.CourseEntities.Update(courseEntity);

        return Task.FromResult(_mapper.Map<Course>(courseEntity));
    }
}