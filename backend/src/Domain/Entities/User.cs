namespace Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string Role { get; private set; }
    public bool IsActive { get; private set; } = true;

    // Navigation properties
    public SellerProfile? SellerProfile { get;  set; }
    public CustomerProfile? CustomerProfile { get;  set; }

    private User() { } // For EF

    public User(string email, string passwordHash, string role)
    {
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
    }

    public void UpdateProfile(string? email = null)
    {
        if (!string.IsNullOrEmpty(email))
            Email = email;

        UpdateTimestamp();
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdateTimestamp();
    }
}
