// Application/Services/StoreService.cs
using Application.DTOs.Store;
using Application.DTOs.Products;
using Domain.Interfaces.Services;
using Application.Mappers;
using Domain.Interfaces;
using Shared.Results;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class StoreService : IStoreService
{
  private readonly IStoreRepository _storeRepository;
  private readonly ISellerProfileRepository _sellerProfileRepository;
  private readonly IProductRepository _productRepository;
  private readonly ILogger<StoreService> _logger;

  public StoreService(
      IStoreRepository storeRepository,
      ISellerProfileRepository sellerProfileRepository,
      IProductRepository productRepository,
      ILogger<StoreService> logger)
  {
    _storeRepository = storeRepository;
    _sellerProfileRepository = sellerProfileRepository;
    _productRepository = productRepository;
    _logger = logger;
  }

  public async Task<Result<StoreResponse>> GetStoreBySellerAsync(string sellerProfileId)
  {
    try
    {
      var store = await _storeRepository.GetBySellerAsync(sellerProfileId);
      if (store == null)
        return Result<StoreResponse>.Failure("Store not found", 404);

      var seller = await _sellerProfileRepository.GetByIdAsync(sellerProfileId);
      var response = store.ToStoreResponse(seller?.User?.Email);
      return Result<StoreResponse>.Success(response);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error retrieving store for seller {SellerId}", sellerProfileId);
      return Result<StoreResponse>.Failure("An error occurred while retrieving store", 500);
    }
  }

  public async Task<Result<StoreResponse>> UpdateStoreAsync(string sellerProfileId, UpdateStoreRequest request)
  {
    try
    {
      var store = await _storeRepository.GetBySellerAsync(sellerProfileId);
      if (store == null)
        return Result<StoreResponse>.Failure("Store not found", 404);

      store.UpdateStore(request.Name, request.Description, request.LogoUrl);
      await _storeRepository.UpdateAsync(store);

      var seller = await _sellerProfileRepository.GetByIdAsync(sellerProfileId);
      var response = store.ToStoreResponse(seller?.User?.Email);

      _logger.LogInformation("Store updated successfully for seller {SellerId}", sellerProfileId);
      return Result<StoreResponse>.Success(response);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error updating store for seller {SellerId}", sellerProfileId);
      return Result<StoreResponse>.Failure("An error occurred while updating store", 500);
    }
  }

  public async Task<Result<IEnumerable<ProductResponse>>> GetStoreProductsAsync(string sellerProfileId)
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
      _logger.LogError(ex, "Error retrieving store products for seller {SellerId}", sellerProfileId);
      return Result<IEnumerable<ProductResponse>>.Failure("An error occurred while retrieving store products", 500);
    }
  }
}
