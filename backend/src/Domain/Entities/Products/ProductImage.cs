namespace Domain.Entities;

public class ProductImage : BaseEntity
{
    public string ProductId { get; private set; }
    public string ImageUrl { get; private set; }
    public bool IsMain { get; private set; }

    // Navigation properties
    public Product Product { get; private set; }

    private ProductImage() { }

    public ProductImage(string productId, string imageUrl, bool isMain = false)
    {
        ProductId = productId;
        ImageUrl = imageUrl;
        IsMain = isMain;
    }
}
