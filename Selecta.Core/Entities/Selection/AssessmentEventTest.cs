namespace Selecta.Core.Entities.Selection;

/// <summary>
/// Liga um AssessmentEvent a um PsychologicalTest aplicado, com um
/// percentual opcional — aba "Testes" na tela de avaliação (ver capturas de
/// tela partilhadas). Corresponde a um registo da tabela
/// "EventoAvaliacaoTestes" já existente.
/// </summary>
public class AssessmentEventTest : Selecta.Core.Entities.IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid AssessmentEventId { get; set; }
    public AssessmentEvent? AssessmentEvent { get; set; }

    public Guid PsychologicalTestId { get; set; }
    public PsychologicalTest? PsychologicalTest { get; set; }

    /// <summary>Corresponde a "Percentual".</summary>
    public int? Percentage { get; set; }
}
