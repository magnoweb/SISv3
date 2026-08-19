namespace Selecta.Core.Entities.Selection;

/// <summary>
/// Componente extra do corpo de um ReportTemplate, além de cabeçalho/rodapé
/// (Header/Footer, já em ReportTemplate). Fecha a última sub-coleção do
/// subsistema de Laudos. Corresponde a um registo da tabela
/// "TipoLaudoComponentes" já existente.
/// </summary>
public class ReportTemplateComponent : Selecta.Core.Entities.IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public int ReportTemplateId { get; set; }
    public ReportTemplate? ReportTemplate { get; set; }

    public Guid ReportComponentId { get; set; }
    public ReportComponent? ReportComponent { get; set; }
}
