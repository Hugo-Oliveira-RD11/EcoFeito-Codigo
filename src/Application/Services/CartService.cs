// Application/Services/CartService.cs
using Application.DTOs.Cart;
using Application.Interfaces.Services;
using Application.Mappers;

using Domain.Entities;
using Domain.Interfaces;

using Microsoft.Extensions.Logging;

using Shared.Results;

namespace Application.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerProfileRepository _customerProfileRepository;
    private readonly ILogger<CartService> _logger;

    public CartService(
        ICartRepository cartRepository,
        IProductRepository productRepository,
        ICustomerProfileRepository customerProfileRepository,
        ILogger<CartService> logger)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _customerProfileRepository = customerProfileRepository;
        _logger = logger;
    }

    public async Task<Result<CartResponse>> GetCartAsync(string customerProfileId)
    {
        try
        {
            var cart = await _cartRepository.GetByCustomerAsync(customerProfileId);
            if (cart == null)
            {
                var customer = await _customerProfileRepository.GetByIdAsync(customerProfileId);
                if (customer == null)
                    return Result<CartResponse>.Failure("Customer not found", 404);

                cart = new Cart(customerProfileId);
                await _cartRepository.AddAsync(cart);
            }

            var response = await cart.ToCartResponse(_productRepository);
            return Result<CartResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving cart for customer {CustomerId}", customerProfileId);
            return Result<CartResponse>.Failure("An error occurred while retrieving cart", 500);
        }
    }

    public async Task<Result<CartResponse>> AddItemToCartAsync(string customerProfileId, AddCartItemRequest request)
    {
        try
        {
            var cart = await _cartRepository.GetByCustomerAsync(customerProfileId);
            if (cart == null)
            {
                var customer = await _customerProfileRepository.GetByIdAsync(customerProfileId);
                if (customer == null)
                    return Result<CartResponse>.Failure("Customer not found", 404);

                cart = new Cart(customerProfileId);
                await _cartRepository.AddAsync(cart);
            }

            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null || !product.IsActive)
                return Result<CartResponse>.Failure("Product not available", 400);

            if (product.StockQuantity < request.Quantity)
                return Result<CartResponse>.Failure("Insufficient stock", 400);

            cart.AddItem(request.ProductId, request.Quantity);
            await _cartRepository.UpdateAsync(cart);

            var response = await cart.ToCartResponse(_productRepository);
            _logger.LogInformation("Item added to cart for customer {CustomerId}", customerProfileId);
            return Result<CartResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding item to cart for customer {CustomerId}", customerProfileId);
            return Result<CartResponse>.Failure("An error occurred while adding item to cart", 500);
        }
    }

    public async Task<Result<CartResponse>> RemoveItemFromCartAsync(string customerProfileId, string productId)
    {
        try
        {
            var cart = await _cartRepository.GetByCustomerAsync(customerProfileId);
            if (cart == null)
                return Result<CartResponse>.Failure("Cart not found", 404);

            cart.RemoveItem(productId);
            await _cartRepository.UpdateAsync(cart);

            var response = await cart.ToCartResponse(_productRepository);
            _logger.LogInformation("Item removed from cart for customer {CustomerId}", customerProfileId);
            return Result<CartResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing item from cart for customer {CustomerId}", customerProfileId);
            return Result<CartResponse>.Failure("An error occurred while removing item from cart", 500);
        }
    }

    public async Task<Result<CartResponse>> UpdateCartItemQuantityAsync(string customerProfileId, string productId, int quantity)
    {
        try
        {
            var cart = await _cartRepository.GetByCustomerAsync(customerProfileId);
            if (cart == null)
                return Result<CartResponse>.Failure("Cart not found", 404);

            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (cartItem == null)
                return Result<CartResponse>.Failure("Item not found in cart", 404);

            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null || !product.IsActive)
                return Result<CartResponse>.Failure("Product not available", 400);

            if (product.StockQuantity < quantity)
                return Result<CartResponse>.Failure("Insufficient stock", 400);

            cartItem.UpdateQuantity(quantity);
            await _cartRepository.UpdateAsync(cart);

            var response = await cart.ToCartResponse(_productRepository);
            _logger.LogInformation("Cart item quantity updated for customer {CustomerId}", customerProfileId);
            return Result<CartResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating cart item quantity for customer {CustomerId}", customerProfileId);
            return Result<CartResponse>.Failure("An error occurred while updating cart item", 500);
        }
    }

    public async Task<Result<bool>> ClearCartAsync(string customerProfileId)
    {
        try
        {
            var cart = await _cartRepository.GetByCustomerAsync(customerProfileId);
            if (cart == null)
                return Result<bool>.Failure("Cart not found", 404);

            cart.Clear();
            await _cartRepository.UpdateAsync(cart);

            _logger.LogInformation("Cart cleared for customer {CustomerId}", customerProfileId);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clearing cart for customer {CustomerId}", customerProfileId);
            return Result<bool>.Failure("An error occurred while clearing cart", 500);
        }
    }
}
