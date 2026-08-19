namespace Selecta.Core.Dtos;

public record CompanyEvaluationResultDto(Guid Id, Guid EvaluationResultId, Guid CompanyId, string Name);

public record CreateCompanyEvaluationResultDto(Guid EvaluationResultId, Guid CompanyId, string Name);

public record UpdateCompanyEvaluationResultDto(Guid Id, Guid EvaluationResultId, Guid CompanyId, string Name);
