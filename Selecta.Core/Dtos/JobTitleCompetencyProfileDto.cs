namespace Selecta.Core.Dtos;

public record JobTitleCompetencyProfileDto(Guid Id, string Name, string? Description, Guid? JobTitleId, string? JobTitleName, bool Active, DateTime CreatedAt);

public record CreateJobTitleCompetencyProfileDto(string Name, string? Description, Guid JobTitleId);

public record UpdateJobTitleCompetencyProfileDto(Guid Id, string Name, string? Description, Guid JobTitleId, bool Active);
