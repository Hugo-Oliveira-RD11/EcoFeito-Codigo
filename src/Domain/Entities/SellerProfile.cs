namespace Domain.Entities;

public class SellerProfile : BaseEntity
{
    public string UserId { get; private set; }
    public string StoreName { get; private set; }
    public string? Description { get; private set; }
    //TODO Refazer para ter uma tabela a parte - Email 
    public string? ContactEmail { get; private set; }
    //TODO Refazer para ter uma tabela a parte - Telefone
    public string? Phone { get; private set; }

    // Navigation properties
    public User User { get; private set; }
    public Store Store { get;  set; }
    public ICollection<Product> Products { get; private set; } = new List<Product>();

    private SellerProfile() { }

    public SellerProfile(string userId, string storeName, string? description = null)
    {
        UserId = userId;
        StoreName = storeName;
        Description = description;
    }

    public void UpdateProfile(string storeName, string? description, string? contactEmail, string? phone)
    {
        StoreName = storeName;
        Description = description;
        ContactEmail = contactEmail;
        Phone = phone;
        UpdateTimestamp();
    }
}
