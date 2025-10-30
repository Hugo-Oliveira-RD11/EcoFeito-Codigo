namespace Application.DTOs.Cart;

public class CartResponse
{
    public string Id { get; set; }
    public string CustomerProfileId { get; set; }
    public List<CartItemResponse> Items { get; set; } = new();
    public decimal TotalAmount { get; set; }
    public int TotalItems { get; set; }
}

public class CartItemResponse
{
    public string Id { get; set; }
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public string ImageUrl { get; set; }
}
