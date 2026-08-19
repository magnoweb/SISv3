using Selecta.Core.Entities.Enums;

namespace Selecta.Core.Dtos;

public record RecruitmentScheduleDto(
    Guid Id, Guid JobOpeningId, string JobOpeningName, Guid TicketId,
    Guid ResponsibleId, string ResponsibleName,
    bool ClientInterview, bool Hired, RecruitmentScheduleType ScheduleType, InterviewResult Result,
    string Name, string Cpf, DateTime Date, TimeSpan Time, ScheduleStatus Status,
    string? InternalNotes, bool HasHistory,
    Guid CreatedById, string CreatedByName, DateTime CreatedAt);

public record CreateRecruitmentScheduleDto(
    Guid JobOpeningId, Guid TicketId, Guid ResponsibleId,
    bool ClientInterview, RecruitmentScheduleType ScheduleType,
    string Name, string Cpf, DateTime Date, TimeSpan Time,
    string? InternalNotes, Guid CreatedById);

public record UpdateRecruitmentScheduleDto(
    Guid Id, Guid JobOpeningId, Guid ResponsibleId,
    bool ClientInterview, bool Hired, RecruitmentScheduleType ScheduleType, InterviewResult Result,
    string Name, string Cpf, DateTime Date, TimeSpan Time, ScheduleStatus Status,
    string? InternalNotes);
