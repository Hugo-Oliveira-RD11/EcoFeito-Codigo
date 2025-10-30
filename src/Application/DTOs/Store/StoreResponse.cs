namespace Application.DTOs.Store;

public class StoreResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string LogoUrl { get; set; }
    public decimal Rating { get; set; }
    public int TotalSales { get; set; }
    public string SellerProfileId { get; set; }
    public string SellerEmail { get; set; }
    public int ProductCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
