using Selecta.Core.Entities.Enums;

namespace Selecta.Core.Dtos;

public record OpinionListEntryDto(
    Guid Id, Guid OpinionListId,
    Guid AssessmentEventId, string CandidateName,
    AssessmentResult Result,
    Guid? EvaluationResultId, string? EvaluationResultName,
    DateTime CreatedAt);

public record CreateOpinionListEntryDto(Guid OpinionListId, Guid AssessmentEventId, AssessmentResult Result, Guid? EvaluationResultId);

public record UpdateOpinionListEntryDto(Guid Id, Guid OpinionListId, Guid AssessmentEventId, AssessmentResult Result, Guid? EvaluationResultId);
