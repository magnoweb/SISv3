namespace Selecta.Core.Dtos;

public record ReportTemplateComponentDto(Guid Id, int ReportTemplateId, string ReportTemplateName, Guid ReportComponentId, string ReportComponentName);

public record CreateReportTemplateComponentDto(int ReportTemplateId, Guid ReportComponentId);

public record UpdateReportTemplateComponentDto(Guid Id, int ReportTemplateId, Guid ReportComponentId);
