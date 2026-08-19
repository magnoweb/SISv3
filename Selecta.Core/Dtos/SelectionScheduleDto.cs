using Selecta.Core.Entities.Enums;

namespace Selecta.Core.Dtos;

public record SelectionScheduleDto(
    Guid Id, Guid? AssessmentEventId,
    Guid JobTitleId, string JobTitleName,
    Guid? ContactId, string? ContactName,
    ServiceOrigin Origin, string? ClientNotes,
    string Name, string? Cpf, DateTime Date, TimeSpan Time, ScheduleStatus Status,
    string? InternalNotes, bool HasHistory,
    Guid CreatedById, string CreatedByName, DateTime CreatedAt);

public record CreateSelectionScheduleDto(
    Guid JobTitleId, Guid? ContactId, ServiceOrigin Origin, string? ClientNotes,
    string Name, string? Cpf, DateTime Date, TimeSpan Time,
    string? InternalNotes, Guid CreatedById);

public record UpdateSelectionScheduleDto(
    Guid Id, Guid? AssessmentEventId, Guid JobTitleId, Guid? ContactId, ServiceOrigin Origin, string? ClientNotes,
    string Name, string? Cpf, DateTime Date, TimeSpan Time, ScheduleStatus Status,
    string? InternalNotes);
