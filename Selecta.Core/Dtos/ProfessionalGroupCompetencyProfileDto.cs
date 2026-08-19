namespace Selecta.Core.Dtos;

public record ProfessionalGroupCompetencyProfileDto(Guid Id, string Name, string? Description, Guid? ProfessionalGroupId, string? ProfessionalGroupName, bool Active, DateTime CreatedAt);

public record CreateProfessionalGroupCompetencyProfileDto(string Name, string? Description, Guid ProfessionalGroupId);

public record UpdateProfessionalGroupCompetencyProfileDto(Guid Id, string Name, string? Description, Guid ProfessionalGroupId, bool Active);
