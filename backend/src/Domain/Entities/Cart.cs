namespace Domain.Entities;

public class Cart : BaseEntity
{
    public string CustomerProfileId { get; private set; }

    // Navigation properties
    public CustomerProfile CustomerProfile { get; private set; }
    public ICollection<CartItem> Items { get; private set; } = new List<CartItem>();

    private Cart() { }

    public Cart(string customerProfileId)
    {
        CustomerProfileId = customerProfileId;
    }

    public void AddItem(string productId, int quantity)
    {
        var existingItem = Items.FirstOrDefault(i => i.ProductId == productId);
        if (existingItem != null)
        {
            existingItem.UpdateQuantity(existingItem.Quantity + quantity);
        }
        else
        {
            var newItem = new CartItem(Id, productId, quantity);
            Items.Add(newItem);
        }
        UpdateTimestamp();
    }

    public void RemoveItem(string productId)
    {
        var item = Items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
        {
            Items.Remove(item);
            UpdateTimestamp();
        }
    }

    public void Clear()
    {
        Items.Clear();
        UpdateTimestamp();
    }
}
