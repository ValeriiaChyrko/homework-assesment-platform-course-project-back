using FluentValidation;
using HomeAssignment.DTOs.RequestDTOs;
using HomeworkAssignment.Application.Abstractions;
using HomeworkAssignment.Application.Abstractions.CategoryRelated;
using HomeworkAssignment.AuthorizationFilters;
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
public class CategoryController(ICategoryService categoryService, HybridCache cache) : ControllerBase
{
    private readonly HybridCacheEntryOptions _cacheOptions = new()
    {
        LocalCacheExpiration = TimeSpan.FromMinutes(30),
        Expiration = TimeSpan.FromHours(1) 
    };

    /// <summary>
    /// Getting a list of categories (cached).
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
    {
        var cachedCategories = await cache.GetOrCreateAsync("categories",
            async _ => await categoryService.GetCategoriesAsync(cancellationToken),
            _cacheOptions, cancellationToken: cancellationToken);

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

        var result = await categoryService.CreateCategoryAsync(request, cancellationToken);
        await cache.RemoveAsync("categories", cancellationToken); 

        return Ok(result);
    }

    /// <summary>
    /// Deleting a category by ID.
    /// </summary>
    [HttpDelete("{categoryId:guid}")]
    [AdminOnly]
    public async Task<IActionResult> Delete(Guid categoryId, CancellationToken cancellationToken = default)
    {
        await categoryService.DeleteCategoryAsync(categoryId, cancellationToken);
        await cache.RemoveAsync("categories", cancellationToken);

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

        var response = await categoryService.UpdateCategoryAsync(categoryId, request, cancellationToken);
        await cache.RemoveAsync("categories", cancellationToken); 

        return Ok(response);
    }
}