using MediatR;

namespace HomeAssignment.Persistence.Commands.Courses;

public sealed record DeleteCourseCommand(Guid Id) : IRequest;