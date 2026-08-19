namespace Selecta.Core.Dtos;

public record ProductivityEntryDto(
    Guid Id, Guid AssessmentEventId, string CandidateName,
    int ActivityId, string ActivityName,
    DateTime Date, int Duration,
    Guid UserId, string UserName,
    DateTime CreatedAt);

public record CreateProductivityEntryDto(Guid AssessmentEventId, int ActivityId, DateTime Date, int Duration, Guid UserId);

public record UpdateProductivityEntryDto(Guid Id, Guid AssessmentEventId, int ActivityId, DateTime Date, int Duration, Guid UserId);
