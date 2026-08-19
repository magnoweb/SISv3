using Selecta.Core.Entities.Enums;

namespace Selecta.Core.Entities.Selection;

/// <summary>
/// Uma linha de uma OpinionList — liga um AssessmentEvent específico a um
/// resultado/parecer. Corresponde a um registo da tabela
/// "ListaParecerEventos" já existente. Reaproveita o enum AssessmentResult
/// (mesmos valores usados em AssessmentEvent.Result) — a coluna "Resultado"
/// tem exatamente o mesmo significado aqui.
/// </summary>
public class OpinionListEntry : Selecta.Core.Entities.IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid OpinionListId { get; set; }
    public OpinionList? OpinionList { get; set; }

    public Guid AssessmentEventId { get; set; }
    public AssessmentEvent? AssessmentEvent { get; set; }

    public AssessmentResult Result { get; set; } = AssessmentResult.NoResult;

    /// <summary>Rótulo customizado opcional. Corresponde a "AvaliacaoResultadoId".</summary>
    public Guid? EvaluationResultId { get; set; }
    public EvaluationResult? EvaluationResult { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
