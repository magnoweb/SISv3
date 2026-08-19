using Selecta.Core.Entities.Enums;

namespace Selecta.Core.Entities.Selection;

/// <summary>
/// Catálogo de atividades (usado por TipoLaudo — ex.: atividade de "produção"
/// e de "leitura" associadas a um tipo de laudo). Corresponde a um registo da
/// tabela "Atividades" já existente.
///
/// Ao contrário da generalidade das entidades desta solução, a chave aqui é
/// <c>int</c> identity (não Guid) — por isso implementa <see cref="IEntity{TKey}"/>
/// diretamente, e não o atalho <see cref="IEntity"/>.
/// </summary>
public class Activity : Selecta.Core.Entities.IEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    /// <summary>Duração estimada (minutos). Corresponde a "Tempo".</summary>
    public int Duration { get; set; }

    /// <summary>Se a duração pode ser ajustada pelo utilizador. Corresponde a "TempoFlexivel".</summary>
    public bool FlexibleDuration { get; set; }

    /// <summary>Corresponde a "Origem".</summary>
    public ServiceOrigin Origin { get; set; }

    /// <summary>Atividade interna do sistema (não removível/editável na UI original). Corresponde a "Sistema".</summary>
    public bool System { get; set; }

    public bool Active { get; set; } = true;
}
