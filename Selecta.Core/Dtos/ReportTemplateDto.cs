namespace Selecta.Core.Dtos;

public record ReportTemplateDto(
    int Id,
    string Name,
    string? Template,
    int ProductionActivityId,
    string ProductionActivityName,
    int ReadingActivityId,
    string ReadingActivityName,
    Guid? HeaderId,
    string? HeaderName,
    Guid? FooterId,
    string? FooterName,
    bool AttachmentReport,
    bool UseCompetencies,
    bool Active);

public record CreateReportTemplateDto(
    string Name, string? Template, int ProductionActivityId, int ReadingActivityId,
    Guid? HeaderId, Guid? FooterId, bool AttachmentReport, bool UseCompetencies);

public record UpdateReportTemplateDto(
    int Id, string Name, string? Template, int ProductionActivityId, int ReadingActivityId,
    Guid? HeaderId, Guid? FooterId, bool AttachmentReport, bool UseCompetencies, bool Active);
