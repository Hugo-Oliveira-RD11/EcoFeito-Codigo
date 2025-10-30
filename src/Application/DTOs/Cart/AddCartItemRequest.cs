namespace Application.DTOs.Cart;

public class AddCartItemRequest
{
    public string ProductId { get; set; }
    public int Quantity { get; set; }
}
