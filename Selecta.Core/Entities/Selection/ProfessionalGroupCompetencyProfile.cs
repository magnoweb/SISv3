using Selecta.Core.Entities.Common;

namespace Selecta.Core.Entities.Selection;

/// <summary>
/// Perfil de competências esperadas para um grupo profissional
/// (ProfessionalGroup). Corresponde ao antigo `PerfilGrupoProfisisonal`
/// (sic — a classe original tinha esse erro de digitação, "Profisisonal"
/// em vez de "Profissional"). O valor de discriminador tem de ser
/// exatamente esse nome de classe, com o erro de digitação incluído,
/// porque é isso que já está gravado na coluna "Discriminator" (convenção
/// padrão do EF6 TPH: o nome literal da classe CLR).
/// </summary>
public class ProfessionalGroupCompetencyProfile : CompetencyProfile
{
    /// <summary>
    /// Nullable porque a coluna "GrupoProfissionalId" é partilhada com o
    /// outro subtipo na mesma tabela. Corresponde a "GrupoProfissionalId".
    /// </summary>
    public Guid? ProfessionalGroupId { get; set; }
    public ProfessionalGroup? ProfessionalGroup { get; set; }
}
