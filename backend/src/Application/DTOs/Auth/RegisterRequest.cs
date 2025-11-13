namespace Application.DTOs.Auth;

public class RegisterRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string StoreName { get; set; }
    public string Description { get; set; }
}
