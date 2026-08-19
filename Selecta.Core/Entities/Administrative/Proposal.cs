using Selecta.Core.Entities.Common;
using Selecta.Core.Entities.Enums;
using Selecta.Core.Entities.Security;

namespace Selecta.Core.Entities.Administrative;

/// <summary>
/// Corresponde a um registo da tabela "Propostas" já existente.
/// Contatos/Observações/Anexos da proposta (existentes na tabela original)
/// ficam fora do escopo desta 1ª fase — mesmo tratamento dado aos
/// sub-módulos equivalentes de JobOpening.
/// </summary>
public class Proposal : Selecta.Core.Entities.IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid ServiceOfferingId { get; set; }
    public ServiceOffering? ServiceOffering { get; set; }

    /// <summary>Corresponde a "EmpresaTempId".</summary>
    public Guid ProspectCompanyId { get; set; }
    public ProspectCompany? ProspectCompany { get; set; }

    /// <summary>Utilizador que criou a proposta. Corresponde a "UsuarioId".</summary>
    public Guid CreatedById { get; set; }
    public User? CreatedBy { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ProposalStatus Status { get; set; } = ProposalStatus.Pending;

    /// <summary>Só preenchido quando Status == Declined. Corresponde a "MotivoRecusa".</summary>
    public DeclineReason? DeclineReason { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
