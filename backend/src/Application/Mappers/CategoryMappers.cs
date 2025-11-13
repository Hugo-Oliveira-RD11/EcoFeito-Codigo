using Application.DTOs.Categories;
using Domain.Entities;

namespace Application.Mappers;

public static class CategoryMappers
{
    public static CategoryResponse ToCategoryResponse(this Category category)
    {
        return new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            ProductCount = category.Products.Count
        };
    }

    public static IEnumerable<CategoryResponse> ToCategoryResponses(this IEnumerable<Category> categories)
    {
        return categories.Select(c => c.ToCategoryResponse());
    }

    public static Category ToCategory(this string name, string description = null)
    {
        return new Category(name, description);
    }
}
