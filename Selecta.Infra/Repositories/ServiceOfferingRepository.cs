using Selecta.Core.Entities.Common;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class ServiceOfferingRepository(SelectaDbContext context) : RepositoryBase<ServiceOffering>(context), IServiceOfferingRepository
{
    protected override string DefaultOrderBy => "Name";
}
