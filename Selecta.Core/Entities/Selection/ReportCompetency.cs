namespace Selecta.Core.Entities.Selection;

/// <summary>
/// Linha de competência de um Report — liga o laudo a uma Competency, com o
/// descritivo usado, a pontuação esperada (perfil) e a pontuação obtida, e
/// um percentual. Corresponde a um registo da tabela "LaudoCompetencias" já
/// existente. Fecha a sub-coleção que tinha ficado de fora quando Report
/// foi portado (dependia de Competency/CompetencyScore, que não existiam
/// ainda nessa altura).
/// </summary>
public class ReportCompetency : Selecta.Core.Entities.IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid ReportId { get; set; }
    public Report? Report { get; set; }

    public Guid CompetencyId { get; set; }
    public Competency? Competency { get; set; }

    public Guid? CompetencyDescriptorId { get; set; }
    public CompetencyDescriptor? CompetencyDescriptor { get; set; }

    /// <summary>Pontuação esperada ("perfil"). Corresponde a "ScoreCompetenciaPerfilId".</summary>
    public Guid? ProfileScoreId { get; set; }
    public CompetencyScore? ProfileScore { get; set; }

    /// <summary>Pontuação obtida. Corresponde a "ScoreCompetenciaId".</summary>
    public Guid? ScoreId { get; set; }
    public CompetencyScore? Score { get; set; }

    /// <summary>Corresponde a "Percentual".</summary>
    public int? Percentage { get; set; }
}
