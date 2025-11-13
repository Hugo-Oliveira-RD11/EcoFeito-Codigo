using Application.DTOs.Auth;
using Shared.Results;

namespace Application.Interfaces.Services;

public interface IAuthService
{
    Task<Result<LoginResponse>> LoginAsync(LoginRequest request);
    Task<Result<RegisterResponse>> RegisterAsync(RegisterRequest request);
}
