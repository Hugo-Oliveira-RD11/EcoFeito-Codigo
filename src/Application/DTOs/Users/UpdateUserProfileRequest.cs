namespace Application.DTOs.Users;

public class UpdateUserProfileRequest
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string StoreName { get; set; }
    public string Description { get; set; }
    public string ContactEmail { get; set; }
}
