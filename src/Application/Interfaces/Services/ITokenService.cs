//TODO Passar isso para Domain
using Domain.Entities;

namespace Application.Interfaces.Services;

public interface ITokenService
{
    string GenerateToken(User user);
}
