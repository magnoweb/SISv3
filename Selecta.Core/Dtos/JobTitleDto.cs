namespace Selecta.Core.Dtos;

public record JobTitleDto(Guid Id, Guid CompanyId, Guid ProfessionalGroupId, string Name, string? Description, bool Active, DateTime CreatedAt);

public record CreateJobTitleDto(Guid CompanyId, Guid ProfessionalGroupId, string Name, string? Description);

public record UpdateJobTitleDto(Guid Id, Guid CompanyId, Guid ProfessionalGroupId, string Name, string? Description, bool Active);
