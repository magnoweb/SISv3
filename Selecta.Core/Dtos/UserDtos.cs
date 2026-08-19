namespace Selecta.Core.Dtos;

public record LoginRequestDto(string Login, string Password);

public record LoginResponseDto(string Token, DateTime ExpiresAt, UserDto User);

public record UserDto(Guid Id, string Name, string Email, string Login, bool IsSystemAdmin, string? Roles);
