using Application.DTOs.Orders;
using Shared.Results;

namespace Application.Interfaces.Services;

public interface IOrderService
{
    Task<Result<OrderResponse>> CreateOrderAsync(string customerProfileId, CreateOrderRequest request);
    Task<Result<OrderResponse>> GetOrderByIdAsync(string orderId, string customerProfileId);
    Task<Result<IEnumerable<OrderResponse>>> GetCustomerOrdersAsync(string customerProfileId);
    Task<Result<IEnumerable<OrderResponse>>> GetSellerOrdersAsync(string sellerProfileId);
    Task<Result<OrderResponse>> UpdateOrderStatusAsync(string orderId, string status);
    Task<Result<bool>> CancelOrderAsync(string orderId, string customerProfileId);
}
