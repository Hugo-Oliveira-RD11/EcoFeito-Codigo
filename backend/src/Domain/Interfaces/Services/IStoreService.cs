using Application.DTOs.Store;
using Application.DTOs.Products;
using Shared.Results;

namespace Application.Interfaces.Services;

public interface IStoreService
{
    Task<Result<StoreResponse>> GetStoreBySellerAsync(string sellerProfileId);
    Task<Result<StoreResponse>> UpdateStoreAsync(string sellerProfileId, UpdateStoreRequest request);
    Task<Result<IEnumerable<ProductResponse>>> GetStoreProductsAsync(string sellerProfileId);
}
