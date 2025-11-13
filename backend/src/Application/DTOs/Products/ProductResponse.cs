namespace Application.DTOs.Products;

public class ProductResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string SellerProfileId { get; set; }
    public string CategoryId { get; set; }
    public bool IsActive { get; set; }
    public string ImageUrl { get; set; }
    public List<string> AdditionalImages { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public string SellerName { get; set; }
    public string CategoryName { get; set; }
}
