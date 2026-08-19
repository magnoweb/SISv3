namespace Selecta.Core.Dtos;

public record ProfessionalGroupDto(Guid Id, string Name, string? Description, bool Active, DateTime CreatedAt);

public record CreateProfessionalGroupDto(string Name, string? Description);

public record UpdateProfessionalGroupDto(Guid Id, string Name, string? Description, bool Active);
