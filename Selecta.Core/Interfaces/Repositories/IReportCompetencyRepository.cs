using Selecta.Core.Entities.Selection;

namespace Selecta.Core.Interfaces.Repositories;

public interface IReportCompetencyRepository : IRepositoryBase<ReportCompetency>
{
    Task<IEnumerable<ReportCompetency>> GetByReportAsync(Guid reportId, CancellationToken ct = default);
}
