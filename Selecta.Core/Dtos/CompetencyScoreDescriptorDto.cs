namespace Selecta.Core.Dtos;

public record CompetencyScoreDescriptorDto(
    Guid Id, Guid CompetencyDescriptorId, string CompetencyDescriptorName,
    Guid CompetencyScoreId, string CompetencyScoreName, string Descriptive, bool Active, DateTime CreatedAt);

public record CreateCompetencyScoreDescriptorDto(Guid CompetencyDescriptorId, Guid CompetencyScoreId, string Descriptive);

public record UpdateCompetencyScoreDescriptorDto(Guid Id, Guid CompetencyDescriptorId, Guid CompetencyScoreId, string Descriptive, bool Active);
