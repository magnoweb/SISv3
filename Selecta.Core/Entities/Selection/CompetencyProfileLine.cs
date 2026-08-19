namespace Selecta.Core.Entities.Selection;

/// <summary>
/// Linha de um CompetencyProfile — liga o perfil (seja de cargo ou de
/// grupo profissional) a uma Competency, com uma pontuação esperada
/// opcional. Corresponde a um registo da tabela "PerfilCompetencias" já
/// existente. A FK para o perfil referencia a base abstrata
/// (CompetencyProfile), já que uma linha pode pertencer a qualquer um dos
/// dois subtipos.
/// </summary>
public class CompetencyProfileLine : Selecta.Core.Entities.IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid CompetencyProfileId { get; set; }
    public CompetencyProfile? CompetencyProfile { get; set; }

    public Guid CompetencyId { get; set; }
    public Competency? Competency { get; set; }

    public Guid? CompetencyScoreId { get; set; }
    public CompetencyScore? CompetencyScore { get; set; }
}
