using Selecta.Core.Entities.Enums;

namespace Selecta.Core.Entities.Selection;

/// <summary>
/// Catálogo de competências avaliáveis (comportamentais/habilidades).
/// Corresponde a um registo da tabela "Competencias" já existente.
///
/// Fora do escopo desta fase: a hierarquia `Perfil`/`PerfilCargo`/
/// `PerfilGrupoProfissional` que referencia competências para montar um
/// "perfil esperado" — usa herança EF6 (TPH, uma classe abstrata com duas
/// subclasses) sem Configuration própria para as subclasses, o que sugere
/// discriminador implícito por convenção; replicar isso às cegas (sem
/// conseguir confirmar contra a BD real qual o nome/valores exatos da
/// coluna discriminadora) tem risco real de mapear errado — fica para
/// quando isso puder ser confirmado.
/// </summary>
public class Competency : Selecta.Core.Entities.IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>Corresponde a "Grupo".</summary>
    public CompetencyGroup Group { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool Active { get; set; } = true;
}
