using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class EvaluationResultRepository(SelectaDbContext context) : RepositoryBase<EvaluationResult>(context), IEvaluationResultRepository
{
    protected override string DefaultOrderBy => "Name";
}
