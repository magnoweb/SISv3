namespace Selecta.Core.Entities.Common;

/// <summary>
/// Corresponde a um registo da tabela "Cidades" já existente na base de dados.
/// Os nomes aqui são em inglês (convenção do novo projeto); o mapeamento para
/// os nomes reais das colunas (em português) fica isolado em
/// Selecta.Infra/Data/Configurations/CityConfiguration.cs — a entidade em si
/// não sabe nada sobre nomes de colunas.
/// </summary>
public class City : Selecta.Core.Entities.IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int Code { get; set; }
    public string Name { get; set; } = string.Empty;

    /// <summary>Sigla do estado (ex.: "SP", "RJ"). Corresponde à coluna "Uf".</summary>
    public string State { get; set; } = string.Empty;
}
