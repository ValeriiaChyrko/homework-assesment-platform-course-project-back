using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Categories;

public record GetCategoryByIdQuery(Guid Id) : IRequest<Category?>;