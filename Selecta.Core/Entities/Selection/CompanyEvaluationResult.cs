using Selecta.Core.Entities.Common;

namespace Selecta.Core.Entities.Selection;

/// <summary>
/// Nome customizado que uma empresa específica usa para um resultado de
/// avaliação do catálogo base (ex.: a empresa X chama "Aprovado" de
/// "Classificado"). Corresponde a um registo de "AvaliacaoResultadosCustom".
/// </summary>
public class CompanyEvaluationResult : Selecta.Core.Entities.IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid EvaluationResultId { get; set; }
    public EvaluationResult? EvaluationResult { get; set; }

    public Guid CompanyId { get; set; }
    public Company? Company { get; set; }

    public string Name { get; set; } = string.Empty;
}
