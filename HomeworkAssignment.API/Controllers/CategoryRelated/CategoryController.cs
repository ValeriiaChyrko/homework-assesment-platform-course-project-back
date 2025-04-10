using FluentValidation;
using HomeAssignment.DTOs.RequestDTOs;
using HomeworkAssignment.Application.Abstractions.CategoryRelated;
using HomeworkAssignment.AuthorizationFilters;
using HomeworkAssignment.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Hybrid;

namespace HomeworkAssignment.Controllers.CategoryRelated;

/// <summary>
/// Controller for managing course categories.
/// </summary>
[Produces("application/json")]
[Authorize]
[ApiController]
[Route("api/categories")]
public class CategoryController(ICategoryService service, HybridCache cache, ICacheKeyManager cacheKeyManager) : ControllerBase
{
    private readonly HybridCacheEntryOptions _cacheOptions = new()
    {
        LocalCacheExpiration = TimeSpan.FromHours(1),
        Expiration = TimeSpan.FromHours(12) 
    };

    /// <summary>
    /// Getting a list of categories.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
    {
        var cacheKey = cacheKeyManager.CategoryList();
        
        var cachedCategories = await cache.GetOrCreateAsync(
            key:cacheKey,
            async _ => await service.GetCategoriesAsync(cancellationToken),
            options:_cacheOptions, 
            tags: [cacheKeyManager.CategoryListGroup()],
            cancellationToken: cancellationToken);

        return Ok(cachedCategories);
    }

    /// <summary>
    /// Creating a new category.
    /// </summary>
    [HttpPost]
    [AdminOnly]
    public async Task<IActionResult> Create(
        [FromBody] RequestCategoryDto request,
        [FromServices] IValidator<RequestCategoryDto> validator,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var result = await service.CreateCategoryAsync(request, cancellationToken);

        await cache.RemoveByTagAsync(cacheKeyManager.CategoryListGroup(), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Deleting a category by ID.
    /// </summary>
    [HttpDelete("{categoryId:guid}")]
    [AdminOnly]
    public async Task<IActionResult> Delete(Guid categoryId, CancellationToken cancellationToken = default)
    {
        await service.DeleteCategoryAsync(categoryId, cancellationToken);
        
        await cache.RemoveByTagAsync(cacheKeyManager.CategoryListGroup(), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Updating a category by ID.
    /// </summary>
    [HttpPut("{categoryId:guid}")]
    [AdminOnly]
    public async Task<IActionResult> Update(
        Guid categoryId,
        [FromBody] RequestCategoryDto request,
        [FromServices] IValidator<RequestCategoryDto> validator,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var response = await service.UpdateCategoryAsync(categoryId, request, cancellationToken);
        
        await cache.RemoveByTagAsync(cacheKeyManager.CategoryListGroup(), cancellationToken);
        return Ok(response);
    }
}