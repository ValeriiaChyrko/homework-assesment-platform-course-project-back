using HomeAssignment.Database.Contexts.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Categories;

public sealed class DeleteCategoryCommandHandler(IHomeworkAssignmentDbContext context)
    : IRequestHandler<DeleteCategoryCommand>
{
    private readonly IHomeworkAssignmentDbContext
        _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var categoryEntity = await _context.CategoryEntities.FindAsync([command.Id], cancellationToken);
        if (categoryEntity != null) _context.CategoryEntities.Remove(categoryEntity);
    }
}