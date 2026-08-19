using Selecta.Core.Entities.Security;

namespace Selecta.Core.Entities.Selection;

/// <summary>
/// Descritivo de uma competência, associado ao utilizador que o redigiu.
/// Corresponde a um registo da tabela "CompetenciaDescritivos" já
/// existente. Fora do escopo desta fase: a sub-coleção
/// ScoreCompetenciaDescritivo (referencia ScoreCompetencia, ainda não
/// portada).
/// </summary>
public class CompetencyDescriptor : Selecta.Core.Entities.IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid CompetencyId { get; set; }
    public Competency? Competency { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }

    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool Active { get; set; } = true;
}
