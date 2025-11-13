using Application.DTOs.Products;
using Shared.Results;

namespace Application.Interfaces.Services;

public interface IProductService
{
    Task<Result<ProductResponse>> CreateProductAsync(CreateProductRequest request, string sellerProfileId);
    Task<Result<ProductResponse>> UpdateProductAsync(string productId, UpdateProductRequest request, string sellerProfileId);
    Task<Result<bool>> DeleteProductAsync(string productId, string sellerProfileId);
    Task<Result<ProductResponse>> GetProductByIdAsync(string productId);
    Task<Result<IEnumerable<ProductResponse>>> GetProductsBySellerAsync(string sellerProfileId);
    Task<Result<IEnumerable<ProductResponse>>> SearchProductsAsync(string searchTerm, string categoryId = null);
    Task<Result<IEnumerable<ProductResponse>>> GetActiveProductsAsync();
}
