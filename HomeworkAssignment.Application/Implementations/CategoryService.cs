using AutoMapper;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.Persistence.Commands.Categories;
using HomeAssignment.Persistence.Queries.Categories;
using HomeworkAssignment.Application.Abstractions;
using HomeworkAssignment.Application.Abstractions.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HomeworkAssignment.Application.Implementations;

public class CategoryService : BaseService<CategoryService>, ICategoryService
{
    private readonly ILogger<CategoryService> _logger;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CategoryService(IDatabaseTransactionManager transactionManager, IMediator mediator,
        ILogger<CategoryService> logger, IMapper mapper)
        : base(logger, transactionManager)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper;
    }


    public async Task<RespondCategoryDto> CreateCategoryAsync(RequestCategoryDto categoryDto, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started creating category: {@CategoryDto}", categoryDto);
        
        var category = _mapper.Map<Category>(categoryDto);
        
        category = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new CreateCategoryCommand(category), cancellationToken),
            cancellationToken: cancellationToken);
        
        _logger.LogInformation("Successfully created category with ID: {CategoryId}", category.Id);
        return _mapper.Map<RespondCategoryDto>(category);
    }

    public async Task<RespondCategoryDto> UpdateCategoryAsync(Guid categoryId, RequestCategoryDto categoryDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started updating category: {@CategoryDto}", categoryDto);
        
        var category = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new GetCategoryByIdQuery(categoryId), cancellationToken),
            cancellationToken: cancellationToken);
        if (category == null)
        {
            _logger.LogWarning("Category with ID: {id} does not exist.", categoryId);
            throw new ArgumentException("Category does not exist.");
        }
        category.Update(
            categoryDto.Name
        );
        
        var updatedCategory = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new UpdateCategoryCommand(categoryId, category), cancellationToken),
            cancellationToken: cancellationToken);
        
        _logger.LogInformation("Successfully updated category: {@CategoryDto}", updatedCategory);
        return _mapper.Map<RespondCategoryDto>(updatedCategory);
    }

    public async Task DeleteCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started deleting category with ID: {CategoryId}", categoryId);
        
        await ExecuteTransactionAsync(
            async () => await _mediator.Send(new DeleteCategoryCommand(categoryId), cancellationToken),
            cancellationToken: cancellationToken);
        
        _logger.LogInformation("Successfully deleted category with ID: {CategoryId}", categoryId);
    }

    public async Task<IReadOnlyCollection<RespondCategoryDto>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started retrieving all categories");
        
        var result = await ExecuteWithExceptionHandlingAsync(
            async () => await _mediator.Send(new GetAllCategoriesQuery(), cancellationToken)
        );

        _logger.LogInformation("Successfully retrieved all categories");
        
        var mappedItems = result.Select(entityModel => _mapper.Map<RespondCategoryDto>(entityModel)).ToList();

        return mappedItems;
    }
}