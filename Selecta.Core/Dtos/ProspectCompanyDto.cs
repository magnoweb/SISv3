namespace Selecta.Core.Dtos;

public record ProspectCompanyDto(Guid Id, Guid? CompanyId, string Name, string Document);

public record CreateProspectCompanyDto(Guid? CompanyId, string Name, string Document);

public record UpdateProspectCompanyDto(Guid Id, Guid? CompanyId, string Name, string Document);
