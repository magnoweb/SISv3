namespace Selecta.Core.Entities.Selection;

/// <summary>
/// Base da hierarquia "perfil de competências esperadas" — o antigo
/// `Perfil` abstrato. Mapeada como TPH (Table Per Hierarchy) para a tabela
/// "Perfis" já existente, confirmado via o schema real (coluna
/// "Discriminator" nvarchar(128) NOT NULL, mais "CargoId" e
/// "GrupoProfissionalId" nullable — cada subtipo só preenche a sua). Ver
/// <see cref="JobTitleCompetencyProfile"/> e
/// <see cref="ProfessionalGroupCompetencyProfile"/>.
/// </summary>
public abstract class CompetencyProfile : Selecta.Core.Entities.IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool Active { get; set; } = true;
}
