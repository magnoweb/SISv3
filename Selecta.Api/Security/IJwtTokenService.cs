using Selecta.Core.Dtos;

namespace Selecta.Api.Security;

public interface IJwtTokenService
{
    (string Token, DateTime ExpiresAt) GenerateToken(UserDto user);
}
