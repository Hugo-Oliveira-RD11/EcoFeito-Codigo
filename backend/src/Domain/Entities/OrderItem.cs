//TODO tem que reimplementar de outras formas
namespace Domain.Entities;

public class OrderItem : BaseEntity
{
  public string OrderId { get; private set; }
  public string ProductId { get; private set; }
  public int Quantity { get; private set; }
  public decimal UnitPrice { get; private set; }

  // Navigation properties
  public Order Order { get; private set; }
  public Product Product { get; private set; }

  private OrderItem() { }

  public OrderItem(string orderId, string productId, int quantity, decimal unitPrice)
  {
    OrderId = orderId;
    ProductId = productId;
    Quantity = quantity;
    UnitPrice = unitPrice;
  }
}
