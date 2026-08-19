using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class PsychologicalTestRepository(SelectaDbContext context) : RepositoryBase<PsychologicalTest>(context), IPsychologicalTestRepository
{
    protected override string DefaultOrderBy => "Name";
}
