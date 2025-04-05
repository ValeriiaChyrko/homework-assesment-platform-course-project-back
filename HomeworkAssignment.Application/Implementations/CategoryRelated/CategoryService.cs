using AutoMapper;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.Persistence.Commands.Categories;
using HomeAssignment.Persistence.Queries.Categories;
using HomeworkAssignment.Application.Abstractions;
using HomeworkAssignment.Application.Abstractions.CategoryRelated;
using HomeworkAssignment.Application.Abstractions.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HomeworkAssignment.Application.Implementations.CategoryRelated;

public class CategoryService(
    IDatabaseTransactionManager transactionManager,
    IMediator mediator,
    ILogger<CategoryService> logger,
    IMapper mapper)
    : BaseService<CategoryService>(logger, transactionManager), ICategoryService
{
    private readonly ILogger<CategoryService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<RespondCategoryDto> CreateCategoryAsync(RequestCategoryDto categoryDto, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Initiating category creation. Data: {@CategoryDto}", categoryDto);
        
        var category = mapper.Map<Category>(categoryDto);
        
        category = await ExecuteTransactionAsync(
            async () => await mediator.Send(new CreateCategoryCommand(category), cancellationToken),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Category created successfully. ID: {CategoryId}, Name: {CategoryName}", category.Id, category.Name);
        return mapper.Map<RespondCategoryDto>(category);
    }

    public async Task<RespondCategoryDto> UpdateCategoryAsync(Guid categoryId, RequestCategoryDto categoryDto, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Initiating category update. ID: {CategoryId}, New Data: {@CategoryDto}", categoryId, categoryDto);
        
        var category = await ExecuteTransactionAsync(
            async () => await mediator.Send(new GetCategoryByIdQuery(categoryId), cancellationToken),
            cancellationToken: cancellationToken);

        if (category == null)
        {
            _logger.LogError("Category update failed. Category with ID {CategoryId} not found.", categoryId);
            throw new ArgumentException($"Category with ID {categoryId} does not exist.");
        }

        category.Update(categoryDto.Name);
        
        var updatedCategory = await ExecuteTransactionAsync(
            async () => await mediator.Send(new UpdateCategoryCommand(categoryId, category), cancellationToken),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Category updated successfully. ID: {CategoryId}, New Name: {CategoryName}", categoryId, updatedCategory.Name);
        return mapper.Map<RespondCategoryDto>(updatedCategory);
    }

    public async Task DeleteCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Initiating category deletion. ID: {CategoryId}", categoryId);
        
        await ExecuteTransactionAsync(
            async () => await mediator.Send(new DeleteCategoryCommand(categoryId), cancellationToken),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Category deleted successfully. ID: {CategoryId}", categoryId);
    }

    public async Task<IReadOnlyCollection<RespondCategoryDto>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving all categories from the database.");
        
        var result = await ExecuteWithExceptionHandlingAsync(
            async () => await mediator.Send(new GetAllCategoriesQuery(), cancellationToken));

        var categories = result as Category[] ?? result.ToArray();
        _logger.LogInformation("Successfully retrieved {CategoryCount} categories.", categories.Length);
        
        return categories.Select(mapper.Map<RespondCategoryDto>).ToList();
    }

    public async Task<RespondCategoryDto?> GetCategoryByCourseIdAsync(Guid courseId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving category for course ID: {CourseId}", courseId);
        
        var result = await ExecuteWithExceptionHandlingAsync(
            async () => await mediator.Send(new GetCategoryByCourseIdQuery(courseId), cancellationToken));

        if (result == null)
        {
            _logger.LogWarning("No category found for course ID: {CourseId}", courseId);
            return null;
        }

        _logger.LogInformation("Successfully retrieved category. ID: {CategoryId}, Name: {CategoryName}", result.Id, result.Name);
        return mapper.Map<RespondCategoryDto>(result);
    }
}