using Selecta.Core.Entities.Enums;

namespace Selecta.Core.Dtos;

public record ContactDto(
    Guid Id, Guid CompanyId, string Name, Gender Gender, string? Position, string? Phone1, string? Phone2,
    string Email, int? BirthDay, int? BirthMonth, string? Notes, bool ReceiveNotifications, bool Active, DateTime CreatedAt);

public record CreateContactDto(
    Guid CompanyId, string Name, Gender Gender, string? Position, string? Phone1, string? Phone2,
    string Email, int? BirthDay, int? BirthMonth, string? Notes, bool ReceiveNotifications);

public record UpdateContactDto(
    Guid Id, Guid CompanyId, string Name, Gender Gender, string? Position, string? Phone1, string? Phone2,
    string Email, int? BirthDay, int? BirthMonth, string? Notes, bool ReceiveNotifications, bool Active);
