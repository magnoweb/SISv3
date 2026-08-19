using Selecta.Core.Entities.Common;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class CollaboratorRepository(SelectaDbContext context) : RepositoryBase<Collaborator>(context), ICollaboratorRepository
{
    protected override string DefaultOrderBy => "Name";
}
