namespace Application.DTOs.Users;

public class UserProfileResponse
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public CustomerProfileDto CustomerProfile { get; set; }
    public SellerProfileDto SellerProfile { get; set; }
}

public class CustomerProfileDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
}

public class SellerProfileDto
{
    public string StoreName { get; set; }
    public string Description { get; set; }
    public string ContactEmail { get; set; }
    public string Phone { get; set; }
}
