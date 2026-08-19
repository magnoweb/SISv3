namespace Selecta.Core.Dtos;

public record ReportCompetencyDto(
    Guid Id,
    Guid ReportId,
    Guid CompetencyId, string CompetencyName,
    Guid? CompetencyDescriptorId, string? CompetencyDescriptorName,
    Guid? ProfileScoreId, string? ProfileScoreName,
    Guid? ScoreId, string? ScoreName,
    int? Percentage);

public record CreateReportCompetencyDto(Guid ReportId, Guid CompetencyId, Guid? CompetencyDescriptorId, Guid? ProfileScoreId, Guid? ScoreId, int? Percentage);

public record UpdateReportCompetencyDto(Guid Id, Guid ReportId, Guid CompetencyId, Guid? CompetencyDescriptorId, Guid? ProfileScoreId, Guid? ScoreId, int? Percentage);
