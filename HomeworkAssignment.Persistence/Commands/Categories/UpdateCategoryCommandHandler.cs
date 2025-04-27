using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Categories;

public sealed class UpdateCategoryCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    : IRequestHandler<UpdateCategoryCommand, Category>
{
    private readonly IHomeworkAssignmentDbContext
        _context = context ?? throw new ArgumentNullException(nameof(context));

    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public Task<Category> Handle(UpdateCategoryCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var categoryEntity = _mapper.Map<CategoryEntity>(command.Category);
        categoryEntity.Id = command.Id;
        _context.CategoryEntities.Update(categoryEntity);

        return Task.FromResult(_mapper.Map<Category>(categoryEntity));
    }
}