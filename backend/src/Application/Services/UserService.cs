// Application/Services/UserService.cs
using Application.DTOs.Users;
using Application.Interfaces.Services;
using Application.Mappers;
using Domain.Interfaces;

using Microsoft.Extensions.Logging;

using Shared.Results;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<Result<UserProfileResponse>> GetUserProfileAsync(string userId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return Result<UserProfileResponse>.Failure("User not found", 404);

            var response = user.ToUserProfileResponse();
            return Result<UserProfileResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user profile for user {UserId}", userId);
            return Result<UserProfileResponse>.Failure("An error occurred while retrieving user profile", 500);
        }
    }

    public async Task<Result<UserProfileResponse>> UpdateUserProfileAsync(string userId, UpdateUserProfileRequest request)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return Result<UserProfileResponse>.Failure("User not found", 404);

            user.UpdateProfile(request.Email);

            // Atualizar perfil específico
            if (user.Role == "Customer" && user.CustomerProfile != null)
            {
                user.CustomerProfile.UpdateProfile(
                    request.FirstName,
                    request.LastName,
                    request.Phone,
                    request.Address
                );
            }
            else if (user.Role == "Seller" && user.SellerProfile != null)
            {
                user.SellerProfile.UpdateProfile(
                    request.StoreName,
                    request.Description,
                    request.ContactEmail,
                    request.Phone
                );

                // Atualizar store também
                if (user.SellerProfile.Store != null)
                {
                    user.SellerProfile.Store.UpdateStore(
                        request.StoreName,
                        request.Description,
                        null // LogoUrl pode ser atualizado separadamente
                    );
                }
            }

            await _userRepository.UpdateAsync(user);

            var response = user.ToUserProfileResponse();
            _logger.LogInformation("User profile updated successfully for user {UserId}", userId);
            return Result<UserProfileResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user profile for user {UserId}", userId);
            return Result<UserProfileResponse>.Failure("An error occurred while updating user profile", 500);
        }
    }
}
