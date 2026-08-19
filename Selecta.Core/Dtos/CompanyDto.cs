using Selecta.Core.Entities.Enums;

namespace Selecta.Core.Dtos;

public record CompanyDto(
    Guid Id, CompanyType Type, string LegalName, string TradeName, string Document,
    string? StateRegistration, string? Address, string? AddressComplement, string? Neighborhood,
    string? CityName, string? State, string? PostalCode, string? Phone1, string? Phone2,
    string? Notes, bool Active, DateTime CreatedAt, Guid? CityId);

public record CreateCompanyDto(
    CompanyType Type, string LegalName, string TradeName, string Document,
    string? StateRegistration, string? Address, string? AddressComplement, string? Neighborhood,
    string? CityName, string? State, string? PostalCode, string? Phone1, string? Phone2,
    string? Notes, Guid? CityId);

public record UpdateCompanyDto(
    Guid Id, CompanyType Type, string LegalName, string TradeName, string Document,
    string? StateRegistration, string? Address, string? AddressComplement, string? Neighborhood,
    string? CityName, string? State, string? PostalCode, string? Phone1, string? Phone2,
    string? Notes, bool Active, Guid? CityId);
