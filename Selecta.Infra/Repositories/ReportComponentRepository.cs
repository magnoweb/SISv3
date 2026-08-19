using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class ReportComponentRepository(SelectaDbContext context) : RepositoryBase<ReportComponent>(context), IReportComponentRepository
{
    protected override string DefaultOrderBy => "Name";
}
