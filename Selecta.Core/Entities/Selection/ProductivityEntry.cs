using Selecta.Core.Entities.Security;

namespace Selecta.Core.Entities.Selection;

/// <summary>
/// Registo de tempo gasto numa Activity, dentro de um AssessmentEvent —
/// aba "Produtividade" na tela de avaliação/laudo (ver capturas de tela
/// partilhadas). Corresponde a um registo da tabela "Produtividades" já
/// existente.
/// </summary>
public class ProductivityEntry : Selecta.Core.Entities.IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid AssessmentEventId { get; set; }
    public AssessmentEvent? AssessmentEvent { get; set; }

    public int ActivityId { get; set; }
    public Activity? Activity { get; set; }

    public DateTime Date { get; set; }

    /// <summary>Tempo gasto, em minutos. Corresponde a "Tempo".</summary>
    public int Duration { get; set; }

    /// <summary>Responsável pelo registo. Corresponde a "UsuarioId".</summary>
    public Guid UserId { get; set; }
    public User? User { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
