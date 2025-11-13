// Application/Services/CategoryService.cs
using Application.DTOs.Categories;
using Domain.Interfaces.Services;
using Application.Mappers;
using Domain.Entities;
using Domain.Interfaces;

using Microsoft.Extensions.Logging;

using Shared.Results;

namespace Application.Services;

public class CategoryService : ICategoryService
{
  private readonly ICategoryRepository _categoryRepository;
  private readonly ILogger<CategoryService> _logger;

  public CategoryService(ICategoryRepository categoryRepository, ILogger<CategoryService> logger)
  {
    _categoryRepository = categoryRepository;
    _logger = logger;
  }

  public async Task<Result<CategoryResponse>> CreateCategoryAsync(CreateCategoryRequest request)
  {
    try
    {
      var existingCategory = await _categoryRepository.GetByNameAsync(request.Name);
      if (existingCategory != null)
        return Result<CategoryResponse>.Failure("Category already exists", 400);

      var category = new Category(request.Name, request.Description);
      await _categoryRepository.AddAsync(category);

      var response = category.ToCategoryResponse();
      _logger.LogInformation("Category {CategoryName} created successfully", category.Name);
      return Result<CategoryResponse>.Success(response);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error creating category {CategoryName}", request.Name);
      return Result<CategoryResponse>.Failure("An error occurred while creating category", 500);
    }
  }

  public async Task<Result<IEnumerable<CategoryResponse>>> GetAllCategoriesAsync()
  {
    try
    {
      var categories = await _categoryRepository.GetAllAsync();
      var response = categories.ToCategoryResponses();
      return Result<IEnumerable<CategoryResponse>>.Success(response);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error retrieving all categories");
      return Result<IEnumerable<CategoryResponse>>.Failure("An error occurred while retrieving categories", 500);
    }
  }

  public async Task<Result<CategoryResponse>> GetCategoryByIdAsync(string categoryId)
  {
    try
    {
      var category = await _categoryRepository.GetByIdAsync(categoryId);
      if (category == null)
        return Result<CategoryResponse>.Failure("Category not found", 404);

      var response = category.ToCategoryResponse();
      return Result<CategoryResponse>.Success(response);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error retrieving category {CategoryId}", categoryId);
      return Result<CategoryResponse>.Failure("An error occurred while retrieving category", 500);
    }
  }
}
