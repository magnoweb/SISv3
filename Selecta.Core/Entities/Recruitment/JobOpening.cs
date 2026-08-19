using Selecta.Core.Entities.Enums;

namespace Selecta.Core.Entities.Recruitment;

/// <summary>
/// Corresponde a um registo da tabela "Vagas" já existente. Ver
/// JobOpeningConfiguration para o mapeamento de nomes de coluna.
///
/// As FKs (Manager/CreatedBy → User, Contact, JobTitle, RecruitmentStage)
/// agora têm navegação real — os quatro módulos relacionados já existem.
///
/// Fora do escopo por agora (existem na tabela/relacionados mas não têm
/// endpoint ainda): Tags, Histórico, Observações, Anexos, Entrevistas com
/// gestor, e o envio de notificação por e-mail ao trocar de etapa
/// (dependiam de Mensagem/Notification na solução original).
/// </summary>
public class JobOpening : Selecta.Core.Entities.IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>Correlaciona esta vaga com um "ticket" de outro módulo (fora do escopo atual). Corresponde a "TicketId".</summary>
    public Guid TicketId { get; set; }

    /// <summary>Utilizador responsável pela vaga (o antigo "Responsavel"). Corresponde a "ResponsavelId".</summary>
    public Guid ManagerId { get; set; }
    public Security.User? Manager { get; set; }

    /// <summary>Contato (cliente) que solicitou a vaga. Corresponde a "ContatoId".</summary>
    public Guid ContactId { get; set; }
    public Common.Contact? Contact { get; set; }

    /// <summary>Cargo/função da vaga. Corresponde a "CargoId".</summary>
    public Guid JobTitleId { get; set; }
    public Common.JobTitle? JobTitle { get; set; }

    /// <summary>Etapa atual do processo de recrutamento. Corresponde a "EtapaRecrutamentoId".</summary>
    public Guid RecruitmentStageId { get; set; }
    public RecruitmentStage? RecruitmentStage { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Summary { get; set; }

    /// <summary>Prazo, em dias. Corresponde a "Prazo".</summary>
    public int DeadlineDays { get; set; }

    public int Quantity { get; set; }
    public decimal? Salary { get; set; }
    public JobOpeningStatus Status { get; set; } = JobOpeningStatus.New;

    /// <summary>Utilizador que criou o registo. Corresponde a "UsuarioId".</summary>
    public Guid CreatedById { get; set; }
    public Security.User? CreatedBy { get; set; }

    public DateTime? PaymentDate { get; set; }
    public DateTime? ClosedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
