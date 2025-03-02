using HomeAssignment.Database.Contexts.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Categories;

public sealed class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
{
    private readonly IHomeworkAssignmentDbContext _context;

    public DeleteCategoryCommandHandler(IHomeworkAssignmentDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
        if (command is null) throw new ArgumentNullException(nameof(command));

        var categoryEntity = await _context.CategoryEntities.FindAsync([command.Id], cancellationToken: cancellationToken);
        if (categoryEntity != null) _context.CategoryEntities.Remove(categoryEntity);
    }
}