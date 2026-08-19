namespace Selecta.Core.Entities.Selection;

/// <summary>
/// Catálogo de resultados possíveis de uma avaliação (ex.: "Aprovado",
/// "Reprovado"). Corresponde a um registo da tabela "AvaliacaoResultados"
/// já existente.
/// </summary>
public class EvaluationResult : Selecta.Core.Entities.IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;

    /// <summary>Valor numérico associado ao resultado (usado para pontuação/ordenação). Corresponde a "Valor".</summary>
    public int Value { get; set; }

    /// <summary>Classe CSS usada para colorir o resultado na UI original. Corresponde a "Class".</summary>
    public string CssClass { get; set; } = string.Empty;
}
