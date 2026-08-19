namespace Selecta.Core.Entities.Common;

/// <summary>
/// Registo de um colaborador interno (distinto de User — este não implica
/// necessariamente acesso ao sistema). Corresponde a um registo da tabela
/// "Colaboradores" já existente.
/// </summary>
public class Collaborator : Selecta.Core.Entities.IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;

    /// <summary>Documento (CPF). Corresponde a "Documento".</summary>
    public string? Document { get; set; }

    public bool Active { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
