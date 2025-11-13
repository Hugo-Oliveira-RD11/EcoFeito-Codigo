using Application.DTOs.Cart;
using Shared.Results;

namespace Application.Interfaces.Services;

public interface ICartService
{
    Task<Result<CartResponse>> GetCartAsync(string customerProfileId);
    Task<Result<CartResponse>> AddItemToCartAsync(string customerProfileId, AddCartItemRequest request);
    Task<Result<CartResponse>> RemoveItemFromCartAsync(string customerProfileId, string productId);
    Task<Result<CartResponse>> UpdateCartItemQuantityAsync(string customerProfileId, string productId, int quantity);
    Task<Result<bool>> ClearCartAsync(string customerProfileId);
}
