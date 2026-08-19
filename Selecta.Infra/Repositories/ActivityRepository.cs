using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class ActivityRepository(SelectaDbContext context) : RepositoryBase<Activity, int>(context), IActivityRepository
{
    protected override string DefaultOrderBy => "Name";
}
