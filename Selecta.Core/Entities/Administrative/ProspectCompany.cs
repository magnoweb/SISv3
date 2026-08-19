using Selecta.Core.Entities.Common;

namespace Selecta.Core.Entities.Administrative;

/// <summary>
/// Empresa "prospect" (ainda não é cliente efetivo) usada para enviar
/// propostas comerciais antes do onboarding completo. Pode opcionalmente
/// já estar ligada a um registo em <see cref="Company"/>. Corresponde a um
/// registo da tabela "EmpresasTemp" já existente.
/// </summary>
public class ProspectCompany : Selecta.Core.Entities.IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>Ligação opcional a uma Company já efetivada. Corresponde a "EmpresaId".</summary>
    public Guid? CompanyId { get; set; }
    public Company? Company { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
}
