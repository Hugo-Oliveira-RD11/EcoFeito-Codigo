using Application.DTOs.Store;
using Domain.Entities;

namespace Application.Mappers;

public static class StoreMappers
{
    public static StoreResponse ToStoreResponse(this Store store, string sellerEmail = null)
    {
        return new StoreResponse
        {
            Id = store.Id,
            Name = store.Name,
            Description = store.Description,
            LogoUrl = store.LogoUrl,
            Rating = store.Rating,
            TotalSales = store.TotalSales,
            SellerProfileId = store.SellerProfileId,
            SellerEmail = sellerEmail,
            ProductCount = store.Products.Count,
            CreatedAt = store.CreatedAt
        };
    }

    public static Store ToStore(this string sellerProfileId, string name, string description = null)
    {
        return new Store(sellerProfileId, name, description);
    }
}
