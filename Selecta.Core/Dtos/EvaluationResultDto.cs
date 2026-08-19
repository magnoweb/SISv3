namespace Selecta.Core.Dtos;

public record EvaluationResultDto(Guid Id, string Name, int Value, string CssClass);

public record CreateEvaluationResultDto(string Name, int Value, string CssClass);

public record UpdateEvaluationResultDto(Guid Id, string Name, int Value, string CssClass);
