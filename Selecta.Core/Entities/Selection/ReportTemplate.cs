namespace Selecta.Core.Entities.Selection;

/// <summary>
/// Modelo de laudo — define o texto base (Template) e as atividades/blocos
/// usados ao gerar um Laudo a partir dele. Corresponde a um registo da
/// tabela "TipoLaudos" já existente. Chave <c>int</c> identity — mesma razão
/// de <see cref="Activity"/>.
///
/// Fora do escopo desta fase: a coleção de Laudos gerados a partir deste
/// modelo (isso é o próprio módulo "Laudo", ainda não portado) e
/// TipoLaudoComponente (lista adicional de componentes do corpo do laudo,
/// além de cabeçalho/rodapé).
/// </summary>
public class ReportTemplate : Selecta.Core.Entities.IEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    /// <summary>Texto/HTML base do modelo. Corresponde a "Modelo".</summary>
    public string? Template { get; set; }

    /// <summary>Atividade de "produção" associada. Corresponde a "AtividadeProducaoId".</summary>
    public int ProductionActivityId { get; set; }
    public Activity? ProductionActivity { get; set; }

    /// <summary>Atividade de "leitura" associada. Corresponde a "AtividadeLeituraId".</summary>
    public int ReadingActivityId { get; set; }
    public Activity? ReadingActivity { get; set; }

    /// <summary>Corresponde a "CabecalhoId".</summary>
    public Guid? HeaderId { get; set; }
    public ReportComponent? Header { get; set; }

    /// <summary>Corresponde a "RodapeId".</summary>
    public Guid? FooterId { get; set; }
    public ReportComponent? Footer { get; set; }

    /// <summary>Se o laudo gerado a partir deste modelo é anexado (vs. gerado inline). Corresponde a "LaudoAnexo".</summary>
    public bool AttachmentReport { get; set; }

    /// <summary>Corresponde a "UsarCompetencias".</summary>
    public bool UseCompetencies { get; set; }

    public bool Active { get; set; } = true;
}
