using Selecta.Core.Entities.Selection;

namespace Selecta.Core.Interfaces.Repositories;

public interface IReportRepository : IRepositoryBase<Report>
{
    Task<Report?> GetByAssessmentEventAsync(Guid assessmentEventId, CancellationToken ct = default);
}
