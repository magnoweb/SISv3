namespace Selecta.Core.Entities.Selection;

/// <summary>
/// Catálogo de níveis/pontuações usados para descrever uma competência
/// (ex.: "Alto"/"AL"/valor 3). Corresponde a um registo da tabela
/// "ScoreCompetencias" já existente.
/// </summary>
public class CompetencyScore : Selecta.Core.Entities.IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;

    /// <summary>Sigla curta (2 caracteres no original). Corresponde a "Sigla".</summary>
    public string Acronym { get; set; } = string.Empty;

    /// <summary>Cor em hexadecimal (ex.: "#FF0000"). Corresponde a "Cor".</summary>
    public string Color { get; set; } = string.Empty;

    /// <summary>Corresponde a "Valor".</summary>
    public int Value { get; set; }

    public string? Description { get; set; }
    public bool Active { get; set; } = true;
}
