namespace Selecta.Core.Dtos;

public record AssessmentEventTestDto(Guid Id, Guid AssessmentEventId, string CandidateName, Guid PsychologicalTestId, string PsychologicalTestName, int? Percentage);

public record CreateAssessmentEventTestDto(Guid AssessmentEventId, Guid PsychologicalTestId, int? Percentage);

public record UpdateAssessmentEventTestDto(Guid Id, Guid AssessmentEventId, Guid PsychologicalTestId, int? Percentage);
