namespace Application.DTOs.Products;

public class CreateProductRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string CategoryId { get; set; }
    public List<string> ImageUrls { get; set; } = new();
}
