using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RequestDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Chapters;

public record GetAllChaptersByCourseIdQuery(Guid CourseId) : IRequest<IEnumerable<Chapter>>;