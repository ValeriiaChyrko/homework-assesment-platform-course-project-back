using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;

namespace HomeworkAssignment.Application.Abstractions.CategoryRelated;

public interface ICategoryService
{
    Task<RespondCategoryDto> CreateCategoryAsync(RequestCategoryDto categoryDto,
        CancellationToken cancellationToken = default);
    Task<RespondCategoryDto> UpdateCategoryAsync(Guid categoryId, RequestCategoryDto categoryDto,
        CancellationToken cancellationToken = default);
    Task DeleteCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default);
    
    Task<IReadOnlyCollection<RespondCategoryDto>> GetCategoriesAsync(CancellationToken cancellationToken = default);
    Task<RespondCategoryDto?> GetCategoryByCourseIdAsync(Guid courseId, CancellationToken cancellationToken = default);
}