using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Categories;

public sealed class CreateCategoryCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    : IRequestHandler<CreateCategoryCommand, Category>
{
    private readonly IHomeworkAssignmentDbContext
        _context = context ?? throw new ArgumentNullException(nameof(context));

    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));


    public async Task<Category> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var categoryEntity = _mapper.Map<CategoryEntity>(command.Category);
        var addedEntity = await _context.CategoryEntities.AddAsync(categoryEntity, cancellationToken);

        return _mapper.Map<Category>(addedEntity.Entity);
    }
}