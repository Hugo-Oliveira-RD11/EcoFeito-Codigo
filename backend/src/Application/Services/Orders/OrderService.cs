// Application/Services/OrderService.cs
using Application.DTOs.Orders;
using Domain.Interfaces.Services;
using Application.Mappers;
using Domain.Entities;
using Domain.Interfaces;

using Microsoft.Extensions.Logging;

using Shared.Results;

namespace Application.Services;

public class OrderService : IOrderService
{
  private readonly IOrderRepository _orderRepository;
  private readonly ICartRepository _cartRepository;
  private readonly IProductRepository _productRepository;
  private readonly ICustomerProfileRepository _customerProfileRepository;
  private readonly ILogger<OrderService> _logger;

  public OrderService(
      IOrderRepository orderRepository,
      ICartRepository cartRepository,
      IProductRepository productRepository,
      ICustomerProfileRepository customerProfileRepository,
      ILogger<OrderService> logger)
  {
    _orderRepository = orderRepository;
    _cartRepository = cartRepository;
    _productRepository = productRepository;
    _customerProfileRepository = customerProfileRepository;
    _logger = logger;
  }

  public async Task<Result<OrderResponse>> CreateOrderAsync(string customerProfileId, CreateOrderRequest request)
  {
    try
    {
      var cart = await _cartRepository.GetByCustomerAsync(customerProfileId);
      if (cart == null || !cart.Items.Any())
        return Result<OrderResponse>.Failure("Cart is empty", 400);

      var customer = await _customerProfileRepository.GetByIdAsync(customerProfileId);
      if (customer == null)
        return Result<OrderResponse>.Failure("Customer not found", 404);

      var order = new Order(customerProfileId, request.ShippingAddress);

      // Validar estoque e adicionar itens
      foreach (var cartItem in cart.Items)
      {
        var product = await _productRepository.GetByIdAsync(cartItem.ProductId);
        if (product == null || !product.IsActive)
          return Result<OrderResponse>.Failure($"Product {cartItem.ProductId} is not available", 400);

        if (product.StockQuantity < cartItem.Quantity)
          return Result<OrderResponse>.Failure($"Insufficient stock for {product.Name}", 400);

        order.AddItem(cartItem.ProductId, cartItem.Quantity, product.Price);

        // Atualizar estoque
        product.UpdateStock(product.StockQuantity - cartItem.Quantity);
        await _productRepository.UpdateAsync(product);
      }

      await _orderRepository.AddAsync(order);

      // Limpar carrinho
      cart.Clear();
      await _cartRepository.UpdateAsync(cart);

      var response = await order.ToOrderResponse(_customerProfileRepository, _productRepository);
      _logger.LogInformation("Order created successfully for customer {CustomerId}", customerProfileId);
      return Result<OrderResponse>.Success(response);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error creating order for customer {CustomerId}", customerProfileId);
      return Result<OrderResponse>.Failure("An error occurred while creating order", 500);
    }
  }

  public async Task<Result<OrderResponse>> GetOrderByIdAsync(string orderId, string customerProfileId)
  {
    try
    {
      var order = await _orderRepository.GetByIdAsync(orderId);
      if (order == null || order.CustomerProfileId != customerProfileId)
        return Result<OrderResponse>.Failure("Order not found", 404);

      var response = await order.ToOrderResponse(_customerProfileRepository, _productRepository);
      return Result<OrderResponse>.Success(response);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error retrieving order {OrderId}", orderId);
      return Result<OrderResponse>.Failure("An error occurred while retrieving order", 500);
    }
  }

  public async Task<Result<IEnumerable<OrderResponse>>> GetCustomerOrdersAsync(string customerProfileId)
  {
    try
    {
      var orders = await _orderRepository.GetByCustomerAsync(customerProfileId);
      var response = await Task.WhenAll(orders.Select(order =>
          order.ToOrderResponse(_customerProfileRepository, _productRepository)));
      return Result<IEnumerable<OrderResponse>>.Success(response);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error retrieving orders for customer {CustomerId}", customerProfileId);
      return Result<IEnumerable<OrderResponse>>.Failure("An error occurred while retrieving orders", 500);
    }
  }

  public async Task<Result<IEnumerable<OrderResponse>>> GetSellerOrdersAsync(string sellerProfileId)
  {
    try
    {
      var orders = await _orderRepository.GetBySellerAsync(sellerProfileId);
      var response = await Task.WhenAll(orders.Select(order =>
          order.ToOrderResponse(_customerProfileRepository, _productRepository)));
      return Result<IEnumerable<OrderResponse>>.Success(response);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error retrieving orders for seller {SellerId}", sellerProfileId);
      return Result<IEnumerable<OrderResponse>>.Failure("An error occurred while retrieving orders", 500);
    }
  }

  public async Task<Result<OrderResponse>> UpdateOrderStatusAsync(string orderId, string status)
  {
    try
    {
      var order = await _orderRepository.GetByIdAsync(orderId);
      if (order == null)
        return Result<OrderResponse>.Failure("Order not found", 404);

      order.UpdateStatus(status);
      await _orderRepository.UpdateAsync(order);

      var response = await order.ToOrderResponse(_customerProfileRepository, _productRepository);
      _logger.LogInformation("Order {OrderId} status updated to {Status}", orderId, status);
      return Result<OrderResponse>.Success(response);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error updating order status for order {OrderId}", orderId);
      return Result<OrderResponse>.Failure("An error occurred while updating order status", 500);
    }
  }

  public async Task<Result<bool>> CancelOrderAsync(string orderId, string customerProfileId)
  {
    try
    {
      var order = await _orderRepository.GetByIdAsync(orderId);
      if (order == null || order.CustomerProfileId != customerProfileId)
        return Result<bool>.Failure("Order not found", 404);

      if (order.Status == "Completed" || order.Status == "Shipped")
        return Result<bool>.Failure("Cannot cancel order in current status", 400);

      order.CancelOrder();

      // Restaurar estoque
      foreach (var item in order.Items)
      {
        var product = await _productRepository.GetByIdAsync(item.ProductId);
        if (product != null)
        {
          product.UpdateStock(product.StockQuantity + item.Quantity);
          await _productRepository.UpdateAsync(product);
        }
      }

      await _orderRepository.UpdateAsync(order);
      _logger.LogInformation("Order {OrderId} cancelled by customer {CustomerId}", orderId, customerProfileId);
      return Result<bool>.Success(true);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error cancelling order {OrderId}", orderId);
      return Result<bool>.Failure("An error occurred while cancelling order", 500);
    }
  }
}
