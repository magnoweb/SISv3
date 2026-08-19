namespace Selecta.Core.Dtos;

public record CompetencyDescriptorDto(Guid Id, Guid CompetencyId, string CompetencyName, Guid UserId, string UserName, string Name, bool Active, DateTime CreatedAt);

public record CreateCompetencyDescriptorDto(Guid CompetencyId, Guid UserId, string Name);

public record UpdateCompetencyDescriptorDto(Guid Id, Guid CompetencyId, Guid UserId, string Name, bool Active);
