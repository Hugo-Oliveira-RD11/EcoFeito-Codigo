namespace Domain.Entities;

public class Store : BaseEntity
{
    public string SellerProfileId { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public string? LogoUrl { get; private set; }
    public decimal Rating { get; private set; }
    public int TotalSales { get; private set; }

    // Navigation properties
    public SellerProfile SellerProfile { get; private set; }
    public ICollection<Product> Products { get; private set; } = new List<Product>();

    private Store() { }

    public Store(string sellerProfileId, string name, string? description = null)
    {
        SellerProfileId = sellerProfileId;
        Name = name;
        Description = description;
    }

    public void UpdateStore(string name, string? description, string? logoUrl)
    {
        Name = name;
        Description = description;
        LogoUrl = logoUrl;
        UpdateTimestamp();
    }

    public void UpdateStats(decimal rating, int totalSales)
    {
        Rating = rating;
        TotalSales = totalSales;
        UpdateTimestamp();
    }
}
