using Selecta.Core.Entities.Enums;

namespace Selecta.Core.Entities.Common;

/// <summary>
/// Corresponde a um registo da tabela "Empresas" já existente. Ver
/// CompanyConfiguration para o mapeamento de nomes de coluna.
///
/// Nota: a tabela original guarda a cidade de duas formas ao mesmo tempo —
/// um texto livre ("Cidade") e, opcionalmente, uma FK normalizada
/// ("CidadeId") para a tabela "Cidades". Mantive as duas aqui
/// (<see cref="CityName"/> e <see cref="CityId"/>/<see cref="City"/>) para
/// não perder dados; o ideal a prazo é migrar tudo para a FK e descontinuar
/// o texto livre.
/// </summary>
public class Company : Selecta.Core.Entities.IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public CompanyType Type { get; set; }
    public string LegalName { get; set; } = string.Empty;
    public string TradeName { get; set; } = string.Empty;

    /// <summary>CNPJ (ou outro documento). Corresponde a "Documento".</summary>
    public string Document { get; set; } = string.Empty;

    public string? StateRegistration { get; set; }
    public string? Address { get; set; }
    public string? AddressComplement { get; set; }
    public string? Neighborhood { get; set; }

    /// <summary>Texto livre legado — ver nota da classe. Corresponde a "Cidade".</summary>
    public string? CityName { get; set; }

    /// <summary>Sigla do estado (ex.: "SP"). Corresponde a "Estado".</summary>
    public string? State { get; set; }

    public string? PostalCode { get; set; }
    public string? Phone1 { get; set; }
    public string? Phone2 { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool Active { get; set; } = true;

    /// <summary>FK normalizada opcional para a tabela "Cidades". Corresponde a "CidadeId".</summary>
    public Guid? CityId { get; set; }
    public City? City { get; set; }
}
