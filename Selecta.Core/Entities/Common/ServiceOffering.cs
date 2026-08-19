namespace Selecta.Core.Entities.Common;

/// <summary>
/// Catálogo dos tipos de serviço que a Selecta oferece (Recrutamento,
/// Seleção, Proposta comercial). Corresponde a um registo da tabela
/// "Servicos" já existente.
/// </summary>
public class ServiceOffering : Selecta.Core.Entities.IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    /// <summary>Corresponde a "Recrutamento".</summary>
    public bool Recruitment { get; set; }

    /// <summary>Corresponde a "Selecao".</summary>
    public bool Selection { get; set; }

    /// <summary>Corresponde a "Proposta".</summary>
    public bool Proposal { get; set; }

    public bool Active { get; set; } = true;
}
