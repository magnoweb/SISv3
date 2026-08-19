namespace Selecta.Core.Entities.Common;

/// <summary>
/// Catálogo de grupos profissionais (usado por JobTitle). Corresponde a um
/// registo da tabela "GruposProfissional" já existente.
/// </summary>
public class ProfessionalGroup : Selecta.Core.Entities.IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool Active { get; set; } = true;
}
