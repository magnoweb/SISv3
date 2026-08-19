using Selecta.Core.Entities.Enums;

namespace Selecta.Core.Entities.Common;

/// <summary>
/// Corresponde a um registo da tabela "Candidatos" já existente. Ver
/// CandidateConfiguration para o mapeamento de nomes de coluna.
/// A tabela original tem ainda mais colunas (endereço, escolaridade,
/// telefone, etc., vindas de outros formulários do sistema antigo) que não
/// foram trazidas nesta 1ª fase — acrescenta-as aqui e na Configuration à
/// medida que forem necessárias.
/// </summary>
public class Candidate : Selecta.Core.Entities.IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public DateTime BirthDate { get; set; }

    /// <summary>CPF (documento brasileiro), só dígitos ou formatado — ver <see cref="Validation.CpfValidator"/>.</summary>
    public string Cpf { get; set; } = string.Empty;

    /// <summary>Número de identidade (RG). Corresponde à coluna "Identidade".</summary>
    public string IdentityDocument { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
