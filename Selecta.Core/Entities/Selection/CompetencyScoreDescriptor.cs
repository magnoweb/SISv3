namespace Selecta.Core.Entities.Selection;

/// <summary>
/// Liga um CompetencyDescriptor a um CompetencyScore com um texto
/// descritivo próprio. Corresponde a um registo da tabela
/// "ScoreCompetenciaDescritivos" já existente.
/// </summary>
public class CompetencyScoreDescriptor : Selecta.Core.Entities.IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid CompetencyDescriptorId { get; set; }
    public CompetencyDescriptor? CompetencyDescriptor { get; set; }

    public Guid CompetencyScoreId { get; set; }
    public CompetencyScore? CompetencyScore { get; set; }

    /// <summary>Corresponde a "Descritivo".</summary>
    public string Descriptive { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool Active { get; set; } = true;
}
