using MediatR;

namespace HomeAssignment.Persistence.Commands.ChapterUserProgresses;

public sealed record DeleteCourseUserProgressCommand(Guid CourseId) : IRequest;