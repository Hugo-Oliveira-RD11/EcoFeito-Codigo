using Application.DTOs.Users;
using Domain.Entities;

namespace Application.Mappers;

public static class UserMappers
{
    public static UserProfileResponse ToUserProfileResponse(this User user)
    {
        var response = new UserProfileResponse
        {
            Id = user.Id,
            Email = user.Email,
            Role = user.Role,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt
        };

        if (user.CustomerProfile != null)
        {
            response.CustomerProfile = new CustomerProfileDto
            {
                FirstName = user.CustomerProfile.FirstName,
                LastName = user.CustomerProfile.LastName,
                Phone = user.CustomerProfile.Phone,
                Address = user.CustomerProfile.Address
            };
        }

        if (user.SellerProfile != null)
        {
            response.SellerProfile = new SellerProfileDto
            {
                StoreName = user.SellerProfile.StoreName,
                Description = user.SellerProfile.Description,
                ContactEmail = user.SellerProfile.ContactEmail,
                Phone = user.SellerProfile.Phone
            };
        }

        return response;
    }

    public static UpdateUserProfileRequest ToUpdateRequest(this User user)
    {
        var request = new UpdateUserProfileRequest
        {
            Email = user.Email
        };

        if (user.CustomerProfile != null)
        {
            request.FirstName = user.CustomerProfile.FirstName;
            request.LastName = user.CustomerProfile.LastName;
            request.Phone = user.CustomerProfile.Phone;
            request.Address = user.CustomerProfile.Address;
        }

        if (user.SellerProfile != null)
        {
            request.StoreName = user.SellerProfile.StoreName;
            request.Description = user.SellerProfile.Description;
            request.ContactEmail = user.SellerProfile.ContactEmail;
            request.Phone = user.SellerProfile.Phone;
        }

        return request;
    }
}
