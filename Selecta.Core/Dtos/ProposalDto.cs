using Selecta.Core.Entities.Enums;

namespace Selecta.Core.Dtos;

public record ProposalDto(
    Guid Id,
    Guid ServiceOfferingId,
    string ServiceOfferingName,
    Guid ProspectCompanyId,
    string ProspectCompanyName,
    Guid CreatedById,
    string Name,
    string? Description,
    ProposalStatus Status,
    DeclineReason? DeclineReason,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    /// <summary>Dias corridos desde a criação até à última atualização (ou até agora). Porta de "Dias.TotalDias".</summary>
    int TotalDays,
    /// <summary>Dias úteis (exclui sáb/dom) no mesmo intervalo. Porta de "Dias.TotalDiasUteis".</summary>
    int TotalWorkingDays);

public record CreateProposalDto(Guid ServiceOfferingId, Guid ProspectCompanyId, Guid CreatedById, string Name, string? Description);

public record UpdateProposalDto(Guid Id, Guid ServiceOfferingId, Guid ProspectCompanyId, string Name, string? Description);

/// <summary>DeclineReason é obrigatório quando NewStatus == Declined; ignorado nos demais casos.</summary>
public record ChangeProposalStatusDto(Guid Id, ProposalStatus NewStatus, DeclineReason? DeclineReason);
