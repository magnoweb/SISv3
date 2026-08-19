using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class CompetencyRepository(SelectaDbContext context) : RepositoryBase<Competency>(context), ICompetencyRepository
{
    protected override string DefaultOrderBy => "Name";
}
