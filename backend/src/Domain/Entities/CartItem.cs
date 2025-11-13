namespace Domain.Entities;
//TODO falta um value
public class CartItem : BaseEntity
{
    public string CartId { get; private set; }
    public string ProductId { get; private set; }
    public int Quantity { get; private set; }

    // Navigation properties
    public Cart Cart { get; private set; }
    public Product Product { get; private set; }

    private CartItem() { }

    public CartItem(string cartId, string productId, int quantity)
    {
        CartId = cartId;
        ProductId = productId;
        Quantity = quantity;
    }

    public void UpdateQuantity(int quantity)
    {
        Quantity = quantity;
        UpdateTimestamp();
    }
}
