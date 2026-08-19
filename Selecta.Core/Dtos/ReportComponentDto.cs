using Selecta.Core.Entities.Enums;

namespace Selecta.Core.Dtos;

public record ReportComponentDto(Guid Id, ComponentType ComponentType, string Name, string? Tag, string? FileName, string? Content, bool Active);

public record CreateReportComponentDto(ComponentType ComponentType, string Name, string? Tag, string? FileName, string? Content);

public record UpdateReportComponentDto(Guid Id, ComponentType ComponentType, string Name, string? Tag, string? FileName, string? Content, bool Active);
