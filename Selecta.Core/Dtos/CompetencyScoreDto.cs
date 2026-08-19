namespace Selecta.Core.Dtos;

public record CompetencyScoreDto(Guid Id, string Name, string Acronym, string Color, int Value, string? Description, bool Active);

public record CreateCompetencyScoreDto(string Name, string Acronym, string Color, int Value, string? Description);

public record UpdateCompetencyScoreDto(Guid Id, string Name, string Acronym, string Color, int Value, string? Description, bool Active);
