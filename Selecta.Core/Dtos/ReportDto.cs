namespace Selecta.Core.Dtos;

public record ReportDto(
    Guid Id,
    Guid AssessmentEventId,
    string CandidateName,
    int ReportTemplateId, string ReportTemplateName,
    string? Descriptive,
    string? FileName,
    DateTime? FileCreatedAt,
    Guid ResponsibleId, string ResponsibleName,
    Guid? SupervisorId, string? SupervisorName,
    Guid? ResponsibleSignatureId, string? ResponsibleSignatureName,
    Guid? SupervisorSignatureId, string? SupervisorSignatureName,
    double? Utilization,
    double? Average,
    DateTime? UpdatedAt,
    DateTime CreatedAt);

/// <summary>
/// AssessmentEventId é a FK real (coluna "EventoAvaliacaoId" na tabela
/// "Laudos") — corrigido depois de confirmar contra o schema real que NÃO
/// é uma relação 1:1 de chave partilhada, ao contrário do que se assumiu
/// inicialmente. Ver nota em Report.cs.
/// </summary>
public record CreateReportDto(Guid AssessmentEventId, int ReportTemplateId, string? Descriptive, Guid ResponsibleId, Guid? SupervisorId);

/// <summary>UpdatedById vem do utilizador autenticado (Blazor resolve via claim, mesmo padrão de JobOpening.CreatedBy) — UpdatedAt é definido no servidor.</summary>
public record UpdateReportDto(
    Guid Id, int ReportTemplateId, string? Descriptive, string? FileName,
    Guid ResponsibleId, Guid? SupervisorId, Guid? ResponsibleSignatureId, Guid? SupervisorSignatureId,
    double? Utilization, double? Average, Guid UpdatedById);
