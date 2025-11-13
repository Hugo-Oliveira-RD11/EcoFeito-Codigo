using Application.DTOs.Products;
using Domain.Entities;

namespace Application.Mappers;

public static class ProductMappers
{
    public static ProductResponse ToProductResponse(this Product product, string sellerName = null, string categoryName = null)
    {
        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            SellerProfileId = product.SellerProfileId,
            CategoryId = product.CategoryId,
            IsActive = product.IsActive,
            ImageUrl = product.ImageUrl,
            AdditionalImages = product.Images.Select(i => i.ImageUrl).ToList(),
            CreatedAt = product.CreatedAt,
            SellerName = sellerName,
            CategoryName = categoryName
        };
    }

    public static IEnumerable<ProductResponse> ToProductResponses(this IEnumerable<Product> products,
        IDictionary<string, string> sellerNames = null, IDictionary<string, string> categoryNames = null)
    {
        return products.Select(p => p.ToProductResponse(
            sellerNames?.GetValueOrDefault(p.SellerProfileId),
            categoryNames?.GetValueOrDefault(p.CategoryId)
        ));
    }

    public static Product ToProduct(this CreateProductRequest request, string sellerProfileId)
    {
        var product = new Product(
            request.Name,
            request.Description,
            request.Price,
            request.StockQuantity,
            sellerProfileId,
            request.CategoryId
        );

        // Adicionar imagens
        if (request.ImageUrls.Any())
        {
            product.ImageUrl = request.ImageUrls.First();
            foreach (var imageUrl in request.ImageUrls.Skip(1))
            {
                product.AddImage(imageUrl);
            }
        }

        return product;
    }
}
