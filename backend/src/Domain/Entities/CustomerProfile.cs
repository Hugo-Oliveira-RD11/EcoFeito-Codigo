namespace Domain.Entities;

public class CustomerProfile : BaseEntity
{
    public string UserId { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    //TODO Separa para uma tabelo diferente - Telefone
    public string? Phone { get; private set; }
    //TODO Separa para uma tabelo diferente - Endereço
    public string? Address { get; private set; }

    // Navigation properties
    public User User { get; private set; }
    public ICollection<Order> Orders { get; private set; } = new List<Order>();
    public Cart Cart { get; private set; }

    private CustomerProfile() { }

    public CustomerProfile(string userId, string firstName, string lastName)
    {
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
    }

    public void UpdateProfile(string firstName, string lastName, string? phone, string? address)
    {
        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
        Address = address;
        UpdateTimestamp();
    }
}
