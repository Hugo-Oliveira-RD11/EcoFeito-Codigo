using Application.DTOs.Users;
using Shared.Results;

namespace Application.Interfaces.Services;

public interface IUserService
{
    Task<Result<UserProfileResponse>> GetUserProfileAsync(string userId);
    Task<Result<UserProfileResponse>> UpdateUserProfileAsync(string userId, UpdateUserProfileRequest request);
}
