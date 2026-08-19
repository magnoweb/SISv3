using Selecta.Core.Entities.Enums;

namespace Selecta.Core.Dtos;

public record JobOpeningDto(
    Guid Id,
    Guid TicketId,
    Guid ManagerId,
    string ManagerName,
    Guid ContactId,
    string ContactName,
    Guid JobTitleId,
    string JobTitleName,
    Guid RecruitmentStageId,
    string RecruitmentStageName,
    string Name,
    string? Summary,
    int DeadlineDays,
    int Quantity,
    decimal? Salary,
    JobOpeningStatus Status,
    Guid CreatedById,
    DateTime? PaymentDate,
    DateTime? ClosedAt,
    DateTime CreatedAt,
    /// <summary>Ex.: "3 days", "2 hours", "Less than 1 hour". Porta de "TempoVaga".</summary>
    string OpenDuration,
    /// <summary>Dias úteis (exclui sáb/dom) desde a criação até ao fecho (ou até agora). Porta de "DiasTrabalhados".</summary>
    int WorkingDaysOpen);

public record CreateJobOpeningDto(
    Guid TicketId,
    Guid ManagerId,
    Guid ContactId,
    Guid JobTitleId,
    Guid RecruitmentStageId,
    string Name,
    string? Summary,
    int DeadlineDays,
    int Quantity,
    decimal? Salary,
    Guid CreatedById);

public record UpdateJobOpeningDto(
    Guid Id,
    Guid ManagerId,
    Guid ContactId,
    Guid JobTitleId,
    Guid RecruitmentStageId,
    string Name,
    string? Summary,
    int DeadlineDays,
    int Quantity,
    decimal? Salary,
    DateTime? PaymentDate);

public record ChangeJobOpeningStatusDto(Guid Id, JobOpeningStatus NewStatus);
