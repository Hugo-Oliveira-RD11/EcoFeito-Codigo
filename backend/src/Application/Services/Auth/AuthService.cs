// Application/Services/AuthService.cs
using Application.DTOs.Auth;
using Domain.Interfaces.Services;
using Domain.Entities;
using Domain.Interfaces;

using Microsoft.Extensions.Logging;

using Shared.Results;

namespace Application.Services;

public class AuthService : IAuthService
{
  private readonly IUserRepository _userRepository;
  private readonly ITokenService _tokenService;
  private readonly IPasswordHasher _passwordHasher;
  private readonly ILogger<AuthService> _logger;

  public AuthService(
      IUserRepository userRepository,
      ITokenService tokenService,
      IPasswordHasher passwordHasher,
      ILogger<AuthService> logger)
  {
    _userRepository = userRepository;
    _tokenService = tokenService;
    _passwordHasher = passwordHasher;
    _logger = logger;
  }

  public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request)
  {
    try
    {
      var user = await _userRepository.GetByEmailAsync(request.Email);
      if (user == null)
        return Result<LoginResponse>.Failure("Invalid credentials", 401);

      if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        return Result<LoginResponse>.Failure("Invalid credentials", 401);

      if (!user.IsActive)
        return Result<LoginResponse>.Failure("Account is deactivated", 401);

      var token = _tokenService.GenerateToken(user);

      var response = new LoginResponse
      {
        Token = token,
        Email = user.Email,
        Role = user.Role,
        UserId = user.Id,
        ExpiresAt = DateTime.UtcNow.AddHours(24)
      };

      _logger.LogInformation("User {Email} logged in successfully", user.Email);
      return Result<LoginResponse>.Success(response);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error during login for email {Email}", request.Email);
      return Result<LoginResponse>.Failure("An error occurred during login", 500);
    }
  }

  public async Task<Result<RegisterResponse>> RegisterAsync(RegisterRequest request)
  {
    try
    {
      if (await _userRepository.EmailExistsAsync(request.Email))
        return Result<RegisterResponse>.Failure("Email already exists", 400);

      var passwordHash = _passwordHasher.HashPassword(request.Password);

      var user = new User(request.Email, passwordHash, request.Role);

      // Criar perfil específico baseado na role
      if (request.Role == "Customer")
      {
        user.CustomerProfile = new CustomerProfile(user.Id, request.FirstName, request.LastName);
      }
      else if (request.Role == "Seller")
      {
        user.SellerProfile = new SellerProfile(user.Id, request.StoreName, request.Description);

        // Criar store automaticamente para o seller
        user.SellerProfile.Store = new Store(user.SellerProfile.Id, request.StoreName, request.Description);
      }

      await _userRepository.AddAsync(user);

      var response = new RegisterResponse
      {
        UserId = user.Id,
        Email = user.Email,
        Role = user.Role
      };

      _logger.LogInformation("User {Email} registered successfully with role {Role}", user.Email, user.Role);
      return Result<RegisterResponse>.Success(response);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error during registration for email {Email}", request.Email);
      return Result<RegisterResponse>.Failure("An error occurred during registration", 500);
    }
  }
}
