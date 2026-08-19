using Selecta.Core.Entities.Common;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class ProfessionalGroupRepository(SelectaDbContext context) : RepositoryBase<ProfessionalGroup>(context), IProfessionalGroupRepository
{
    protected override string DefaultOrderBy => "Name";
}
