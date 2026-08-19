using Selecta.Core.Entities.Enums;

namespace Selecta.Core.Entities.Selection;

/// <summary>
/// Bloco reutilizável de um modelo de laudo (relatório) — cabeçalho, rodapé,
/// tag ou conteúdo estático/dinâmico. Corresponde a um registo da tabela
/// "LaudoComponentes" já existente. Referenciado por TipoLaudo (ainda não
/// portado) para montar o cabeçalho/rodapé de um modelo.
/// </summary>
public class ReportComponent : Selecta.Core.Entities.IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public ComponentType ComponentType { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Tag { get; set; }

    /// <summary>Corresponde a "Arquivo".</summary>
    public string? FileName { get; set; }

    /// <summary>Corresponde a "Conteudo".</summary>
    public string? Content { get; set; }

    public bool Active { get; set; } = true;
}
