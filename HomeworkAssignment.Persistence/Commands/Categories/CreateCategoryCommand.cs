using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Categories;

public sealed record CreateCategoryCommand(Category Category) : IRequest<Category>;