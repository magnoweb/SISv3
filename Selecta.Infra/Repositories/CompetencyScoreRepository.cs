using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class CompetencyScoreRepository(SelectaDbContext context) : RepositoryBase<CompetencyScore>(context), ICompetencyScoreRepository
{
    protected override string DefaultOrderBy => "Value";
}
