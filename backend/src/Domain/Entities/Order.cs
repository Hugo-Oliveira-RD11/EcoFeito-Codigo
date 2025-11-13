//TODO tem que reimplementar de outras formas
namespace Domain.Entities;

public class Order : BaseEntity
{
  public string CustomerProfileId { get; private set; }
  public decimal TotalAmount { get; private set; }
  public string Status { get; private set; } = "Pending";
  public string? ShippingAddress { get; private set; }

  // Navigation properties
  public CustomerProfile CustomerProfile { get; private set; }
  public ICollection<OrderItem> Items { get; private set; } = new List<OrderItem>();

  private Order() { }

  public Order(string customerProfileId, string? shippingAddress = null)
  {
    CustomerProfileId = customerProfileId;
    ShippingAddress = shippingAddress;
  }

  public void AddItem(string productId, int quantity, decimal unitPrice)
  {
    var item = new OrderItem(Id, productId, quantity, unitPrice);
    Items.Add(item);
    TotalAmount += quantity * unitPrice;
  }

  public void UpdateStatus(string status)
  {
    Status = status;
    UpdateTimestamp();
  }

  public void CompleteOrder()
  {
    Status = "Completed";
    UpdateTimestamp();
  }

  public void CancelOrder()
  {
    Status = "Cancelled";
    UpdateTimestamp();
  }
}
