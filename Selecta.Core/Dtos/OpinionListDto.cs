namespace Selecta.Core.Dtos;

public record OpinionListDto(
    Guid Id, string Code,
    Guid ContactId, string ContactName,
    Guid ResponsibleId, string ResponsibleName,
    DateTime Date, string? Notes,
    Guid CreatedById, string CreatedByName,
    DateTime CreatedAt);

/// <summary>Code é gerado no servidor (formato "yyyyMMdd_HHmmss") — não faz parte do Create.</summary>
public record CreateOpinionListDto(Guid ContactId, Guid ResponsibleId, DateTime Date, string? Notes, Guid CreatedById);

public record UpdateOpinionListDto(Guid Id, Guid ContactId, Guid ResponsibleId, DateTime Date, string? Notes);
