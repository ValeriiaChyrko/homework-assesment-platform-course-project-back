using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Categories;

public record GetAllCategoriesQuery : IRequest<IEnumerable<Category>>;