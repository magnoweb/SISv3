namespace Selecta.Core.Entities.Selection;

/// <summary>
/// Catálogo de testes psicológicos aplicáveis numa avaliação. Corresponde a
/// um registo da tabela "TestesPsicologico" já existente.
/// </summary>
public class PsychologicalTest : Selecta.Core.Entities.IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool Active { get; set; } = true;
}
