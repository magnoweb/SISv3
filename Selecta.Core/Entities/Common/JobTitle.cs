namespace Selecta.Core.Entities.Common;

/// <summary>
/// Corresponde a um registo da tabela "Cargos" já existente (catálogo de
/// cargos de uma empresa, usado pelas vagas).
/// </summary>
public class JobTitle : Selecta.Core.Entities.IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CompanyId { get; set; }
    public Company? Company { get; set; }

    /// <summary>Corresponde a "GrupoProfissionalId".</summary>
    public Guid ProfessionalGroupId { get; set; }
    public ProfessionalGroup? ProfessionalGroup { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool Active { get; set; } = true;
}
