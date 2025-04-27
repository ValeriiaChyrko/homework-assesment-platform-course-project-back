using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Categories;

public sealed record UpdateCategoryCommand(Guid Id, Category Category) : IRequest<Category>;