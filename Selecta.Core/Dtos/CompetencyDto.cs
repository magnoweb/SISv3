using Selecta.Core.Entities.Enums;

namespace Selecta.Core.Dtos;

public record CompetencyDto(Guid Id, CompetencyGroup Group, string Name, string? Description, bool Active, DateTime CreatedAt);

public record CreateCompetencyDto(CompetencyGroup Group, string Name, string? Description);

public record UpdateCompetencyDto(Guid Id, CompetencyGroup Group, string Name, string? Description, bool Active);
