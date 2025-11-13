using Application.DTOs.Cart;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Mappers;

public static class CartMappers
{
    public static async Task<CartResponse> ToCartResponse(this Cart cart, IProductRepository productRepository)
    {
        var response = new CartResponse
        {
            Id = cart.Id,
            CustomerProfileId = cart.CustomerProfileId,
            Items = new List<CartItemResponse>(),
            TotalAmount = 0,
            TotalItems = 0
        };

        foreach (var item in cart.Items)
        {
            var product = await productRepository.GetByIdAsync(item.ProductId);
            if (product != null && product.IsActive)
            {
                var itemResponse = item.ToCartItemResponse(product);
                response.Items.Add(itemResponse);
                response.TotalAmount += itemResponse.TotalPrice;
                response.TotalItems += item.Quantity;
            }
        }

        return response;
    }

    public static CartItemResponse ToCartItemResponse(this CartItem item, Product product)
    {
        return new CartItemResponse
        {
            Id = item.Id,
            ProductId = item.ProductId,
            ProductName = product.Name,
            UnitPrice = product.Price,
            Quantity = item.Quantity,
            TotalPrice = item.Quantity * product.Price,
            ImageUrl = product.ImageUrl
        };
    }

    public static CartItem ToCartItem(this AddCartItemRequest request, string cartId)
    {
        return new CartItem(cartId, request.ProductId, request.Quantity);
    }
}
