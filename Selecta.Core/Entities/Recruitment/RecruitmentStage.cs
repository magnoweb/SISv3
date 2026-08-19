namespace Selecta.Core.Entities.Recruitment;

/// <summary>
/// Corresponde a um registo da tabela "EtapasRecrutamento" já existente —
/// catálogo ordenado das etapas pelas quais uma vaga (JobOpening) passa.
/// </summary>
public class RecruitmentStage : Selecta.Core.Entities.IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    /// <summary>Classe CSS usada para colorir a etapa na UI original. Corresponde a "EtapaCss".</summary>
    public string? CssClass { get; set; }

    /// <summary>Posição na sequência de etapas. Corresponde a "Ordem".</summary>
    public int Order { get; set; }

    public bool Active { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
