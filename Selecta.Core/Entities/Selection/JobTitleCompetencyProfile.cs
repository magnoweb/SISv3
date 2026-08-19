using Selecta.Core.Entities.Common;

namespace Selecta.Core.Entities.Selection;

/// <summary>
/// Perfil de competências esperadas para um cargo (JobTitle). Corresponde
/// ao antigo `PerfilCargo` — valor de discriminador confirmado no schema
/// real: "PerfilCargo" (nome exato da classe CLR original, convenção
/// padrão do EF6 TPH).
/// </summary>
public class JobTitleCompetencyProfile : CompetencyProfile
{
    /// <summary>
    /// Nullable porque a coluna "CargoId" é partilhada com o outro subtipo
    /// na mesma tabela (só preenchida quando Discriminator = "PerfilCargo").
    /// Corresponde a "CargoId".
    /// </summary>
    public Guid? JobTitleId { get; set; }
    public JobTitle? JobTitle { get; set; }
}
