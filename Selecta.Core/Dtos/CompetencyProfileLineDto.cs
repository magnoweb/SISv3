namespace Selecta.Core.Dtos;

public record CompetencyProfileLineDto(Guid Id, Guid CompetencyProfileId, string CompetencyProfileName, Guid CompetencyId, string CompetencyName, Guid? CompetencyScoreId, string? CompetencyScoreName);

public record CreateCompetencyProfileLineDto(Guid CompetencyProfileId, Guid CompetencyId, Guid? CompetencyScoreId);

public record UpdateCompetencyProfileLineDto(Guid Id, Guid CompetencyProfileId, Guid CompetencyId, Guid? CompetencyScoreId);
