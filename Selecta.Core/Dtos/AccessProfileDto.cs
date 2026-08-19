namespace Selecta.Core.Dtos;

public record AccessProfileDto(Guid Id, string Name, string? Description);

public record CreateAccessProfileDto(string Name, string? Description);

public record UpdateAccessProfileDto(Guid Id, string Name, string? Description);
