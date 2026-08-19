using Selecta.Core.Entities.Security;

namespace Selecta.Core.Entities.Selection;

/// <summary>
/// Laudo gerado a partir de um AssessmentEvent + ReportTemplate. Corresponde
/// a um registo da tabela "Laudos" já existente.
///
/// CORREÇÃO (confirmada contra o schema real da BD): esta relação NÃO é
/// 1:1 de chave partilhada como se assumiu inicialmente — a tabela "Laudos"
/// tem uma coluna "EventoAvaliacaoId" própria, separada de "LaudoId" (a
/// FK real é <see cref="AssessmentEventId"/>, não <see cref="Id"/>). Não
/// existe sequer índice único nessa coluna na BD — "no máximo um Report
/// por AssessmentEvent" é só uma regra de aplicação (ver
/// ReportService.CreateAsync), nunca foi garantida pelo banco.
///
/// Fora do escopo desta fase: LaudoCompetencia (já portado como
/// ReportCompetency) e TipoLaudoComponente (ainda não portado).
/// </summary>
public class Report : Selecta.Core.Entities.IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid AssessmentEventId { get; set; }
    public AssessmentEvent? AssessmentEvent { get; set; }

    public int ReportTemplateId { get; set; }
    public ReportTemplate? ReportTemplate { get; set; }

    /// <summary>Texto/narrativa do laudo. Corresponde a "Descritivo".</summary>
    public string? Descriptive { get; set; }

    /// <summary>Corresponde a "Arquivo".</summary>
    public string? FileName { get; set; }

    /// <summary>Corresponde a "ArquivoDataInclusao".</summary>
    public DateTime? FileCreatedAt { get; set; }

    public Guid ResponsibleId { get; set; }
    public User? Responsible { get; set; }

    public Guid? SupervisorId { get; set; }
    public User? Supervisor { get; set; }

    /// <summary>Corresponde a "ResponsavelAssinaturaId".</summary>
    public Guid? ResponsibleSignatureId { get; set; }
    public User? ResponsibleSignature { get; set; }

    /// <summary>Corresponde a "SupervisorAssinaturaId".</summary>
    public Guid? SupervisorSignatureId { get; set; }
    public User? SupervisorSignature { get; set; }

    /// <summary>Corresponde a "Aproveitamento".</summary>
    public double? Utilization { get; set; }

    /// <summary>Corresponde a "Media".</summary>
    public double? Average { get; set; }

    /// <summary>Corresponde a "AtualizacaoUsuarioId".</summary>
    public Guid? UpdatedById { get; set; }
    public User? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
