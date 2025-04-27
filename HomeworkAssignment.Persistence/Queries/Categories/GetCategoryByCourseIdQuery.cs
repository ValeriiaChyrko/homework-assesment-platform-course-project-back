using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Categories;

public record GetCategoryByCourseIdQuery(Guid CourseId) : IRequest<Category?>;