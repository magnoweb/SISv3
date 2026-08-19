using Selecta.Core.Entities.Selection;

namespace Selecta.Core.Interfaces.Repositories;

public interface IProductivityEntryRepository : IRepositoryBase<ProductivityEntry>
{
    Task<IEnumerable<ProductivityEntry>> GetByAssessmentEventAsync(Guid assessmentEventId, CancellationToken ct = default);
}
