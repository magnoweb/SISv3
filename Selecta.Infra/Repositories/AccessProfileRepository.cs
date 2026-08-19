using Selecta.Core.Entities.Security;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class AccessProfileRepository(SelectaDbContext context) : RepositoryBase<AccessProfile>(context), IAccessProfileRepository
{
    protected override string DefaultOrderBy => "Name";
}
