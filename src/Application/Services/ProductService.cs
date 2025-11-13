// Application/Services/ProductService.cs
using Application.DTOs.Products;
using Application.Interfaces.Services;
using Application.Mappers;
using Domain.Interfaces;

using Microsoft.Extensions.Logging;

using Shared.Results;

namespace Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ISellerProfileRepository _sellerProfileRepository;
    private readonly ILogger<ProductService> _logger;

    public ProductService(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        ISellerProfileRepository sellerProfileRepository,
        ILogger<ProductService> logger)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _sellerProfileRepository = sellerProfileRepository;
        _logger = logger;
    }

    public async Task<Result<ProductResponse>> CreateProductAsync(CreateProductRequest request, string sellerProfileId)
    {
        try
        {
            var seller = await _sellerProfileRepository.GetByIdAsync(sellerProfileId);
            if (seller == null)
                return Result<ProductResponse>.Failure("Seller not found", 404);

            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
            if (category == null)
                return Result<ProductResponse>.Failure("Category not found", 404);

            var product = request.ToProduct(sellerProfileId);
            await _productRepository.AddAsync(product);

            var response = product.ToProductResponse(seller.StoreName, category.Name);
            _logger.LogInformation("Product {ProductName} created successfully by seller {SellerId}", product.Name, sellerProfileId);
            return Result<ProductResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product for seller {SellerId}", sellerProfileId);
            return Result<ProductResponse>.Failure("An error occurred while creating product", 500);
        }
    }

    public async Task<Result<ProductResponse>> UpdateProductAsync(string productId, UpdateProductRequest request, string sellerProfileId)
    {
        try
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null || product.SellerProfileId != sellerProfileId)
                return Result<ProductResponse>.Failure("Product not found", 404);

            product.UpdateProduct(
                request.Name,
                request.Description,
                request.Price,
                request.StockQuantity,
                request.CategoryId
            );

            await _productRepository.UpdateAsync(product);

            var seller = await _sellerProfileRepository.GetByIdAsync(sellerProfileId);
            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);

            var response = product.ToProductResponse(seller?.StoreName, category?.Name);
            _logger.LogInformation("Product {ProductId} updated successfully", productId);
            return Result<ProductResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product {ProductId}", productId);
            return Result<ProductResponse>.Failure("An error occurred while updating product", 500);
        }
    }

    public async Task<Result<bool>> DeleteProductAsync(string productId, string sellerProfileId)
    {
        try
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null || product.SellerProfileId != sellerProfileId)
                return Result<bool>.Failure("Product not found", 404);

            product.Deactivate();
            await _productRepository.UpdateAsync(product);

            _logger.LogInformation("Product {ProductId} deactivated successfully", productId);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product {ProductId}", productId);
            return Result<bool>.Failure("An error occurred while deleting product", 500);
        }
    }

    public async Task<Result<ProductResponse>> GetProductByIdAsync(string productId)
    {
        try
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                return Result<ProductResponse>.Failure("Product not found", 404);

            var seller = await _sellerProfileRepository.GetByIdAsync(product.SellerProfileId);
            var category = await _categoryRepository.GetByIdAsync(product.CategoryId);

            var response = product.ToProductResponse(seller?.StoreName, category?.Name);
            return Result<ProductResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving product {ProductId}", productId);
            return Result<ProductResponse>.Failure("An error occurred while retrieving product", 500);
        }
    }

    public async Task<Result<IEnumerable<ProductResponse>>> GetProductsBySellerAsync(string sellerProfileId)
    {
        try
        {
            var products = await _productRepository.GetBySellerAsync(sellerProfileId);
            var seller = await _sellerProfileRepository.GetByIdAsync(sellerProfileId);

            var sellerNames = new Dictionary<string, string> { [sellerProfileId] = seller?.StoreName };
            var response = products.ToProductResponses(sellerNames);

            return Result<IEnumerable<ProductResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving products for seller {SellerId}", sellerProfileId);
            return Result<IEnumerable<ProductResponse>>.Failure("An error occurred while retrieving products", 500);
        }
    }

    public async Task<Result<IEnumerable<ProductResponse>>> SearchProductsAsync(string searchTerm, string categoryId = null)
    {
        try
        {
            var products = await _productRepository.SearchAsync(searchTerm, categoryId);

            var sellerIds = products.Select(p => p.SellerProfileId).Distinct();
            var categoryIds = products.Select(p => p.CategoryId).Distinct();

            var sellers = await _sellerProfileRepository.GetByIdsAsync(sellerIds);
            var categories = await _categoryRepository.GetByIdsAsync(categoryIds);

            var sellerNames = sellers.ToDictionary(s => s.Id, s => s.StoreName);
            var categoryNames = categories.ToDictionary(c => c.Id, c => c.Name);

            var response = products.ToProductResponses(sellerNames, categoryNames);
            return Result<IEnumerable<ProductResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching products with term {SearchTerm}", searchTerm);
            return Result<IEnumerable<ProductResponse>>.Failure("An error occurred while searching products", 500);
        }
    }

    public async Task<Result<IEnumerable<ProductResponse>>> GetActiveProductsAsync()
    {
        try
        {
            var products = await _productRepository.GetActiveProductsAsync();

            var sellerIds = products.Select(p => p.SellerProfileId).Distinct();
            var categoryIds = products.Select(p => p.CategoryId).Distinct();

            var sellers = await _sellerProfileRepository.GetByIdsAsync(sellerIds);
            var categories = await _categoryRepository.GetByIdsAsync(categoryIds);

            var sellerNames = sellers.ToDictionary(s => s.Id, s => s.StoreName);
            var categoryNames = categories.ToDictionary(c => c.Id, c => c.Name);

            var response = products.ToProductResponses(sellerNames, categoryNames);
            return Result<IEnumerable<ProductResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving active products");
            return Result<IEnumerable<ProductResponse>>.Failure("An error occurred while retrieving products", 500);
        }
    }
}
