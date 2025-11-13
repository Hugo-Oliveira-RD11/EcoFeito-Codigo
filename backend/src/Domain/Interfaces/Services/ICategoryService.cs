using Application.DTOs.Categories;
using Shared.Results;

namespace Application.Interfaces.Services;

public interface ICategoryService
{
    Task<Result<CategoryResponse>> CreateCategoryAsync(CreateCategoryRequest request);
    Task<Result<IEnumerable<CategoryResponse>>> GetAllCategoriesAsync();
    Task<Result<CategoryResponse>> GetCategoryByIdAsync(string categoryId);
}
