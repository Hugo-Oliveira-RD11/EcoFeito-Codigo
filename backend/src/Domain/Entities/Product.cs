namespace Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public int StockQuantity { get; private set; }
    public string SellerProfileId { get; private set; }
    public string CategoryId { get; private set; }
    public bool IsActive { get; private set; } = true;
    public string? ImageUrl { get; private set; }

    // Navigation properties
    public SellerProfile SellerProfile { get; private set; }
    public Category Category { get; private set; }
    public ICollection<ProductImage> Images { get; private set; } = new List<ProductImage>();
    public ICollection<CartItem> CartItems { get; private set; } = new List<CartItem>();
    public ICollection<OrderItem> OrderItems { get; private set; } = new List<OrderItem>();

    private Product() { }

    public Product(string name, string description, decimal price, int stockQuantity,
                  string sellerProfileId, string categoryId)
    {
        Name = name;
        Description = description;
        Price = price;
        StockQuantity = stockQuantity;
        SellerProfileId = sellerProfileId;
        CategoryId = categoryId;
    }

    public void UpdateProduct(string name, string description, decimal price, int stockQuantity, string categoryId)
    {
        Name = name;
        Description = description;
        Price = price;
        StockQuantity = stockQuantity;
        CategoryId = categoryId;
        UpdateTimestamp();
    }

    public void UpdateStock(int quantity)
    {
        StockQuantity = quantity;
        UpdateTimestamp();
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdateTimestamp();
    }

    public void AddImage(string imageUrl, bool isMain = false)
    {
        var image = new ProductImage(Id, imageUrl, isMain);
        Images.Add(image);
    }
}
