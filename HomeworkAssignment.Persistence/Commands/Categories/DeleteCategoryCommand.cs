using MediatR;

namespace HomeAssignment.Persistence.Commands.Categories;

public sealed record DeleteCategoryCommand(Guid Id) : IRequest;