using Application.DTOs.Orders;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Mappers;

public static class OrderMappers
{
    public static async Task<OrderResponse> ToOrderResponse(this Order order,
        ICustomerProfileRepository customerProfileRepository,
        IProductRepository productRepository)
    {
        var customer = await customerProfileRepository.GetByIdAsync(order.CustomerProfileId);
        var customerName = customer != null ? $"{customer.FirstName} {customer.LastName}" : "Unknown";

        var response = new OrderResponse
        {
            Id = order.Id,
            CustomerProfileId = order.CustomerProfileId,
            CustomerName = customerName,
            TotalAmount = order.TotalAmount,
            Status = order.Status,
            ShippingAddress = order.ShippingAddress,
            Items = new List<OrderItemResponse>(),
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt
        };

        foreach (var item in order.Items)
        {
            var product = await productRepository.GetByIdAsync(item.ProductId);
            var itemResponse = item.ToOrderItemResponse(product);
            response.Items.Add(itemResponse);
        }

        return response;
    }

    public static OrderItemResponse ToOrderItemResponse(this OrderItem item, Product product)
    {
        return new OrderItemResponse
        {
            Id = item.Id,
            ProductId = item.ProductId,
            ProductName = product?.Name ?? "Unknown Product",
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice,
            TotalPrice = item.Quantity * item.UnitPrice
        };
    }

    public static IEnumerable<OrderResponse> ToOrderResponses(this IEnumerable<Order> orders,
        ICustomerProfileRepository customerProfileRepository,
        IProductRepository productRepository)
    {
        var tasks = orders.Select(order => order.ToOrderResponse(customerProfileRepository, productRepository));
        return Task.WhenAll(tasks).Result;
    }
}
