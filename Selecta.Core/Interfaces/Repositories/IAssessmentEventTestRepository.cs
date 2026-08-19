using Selecta.Core.Entities.Selection;

namespace Selecta.Core.Interfaces.Repositories;

public interface IAssessmentEventTestRepository : IRepositoryBase<AssessmentEventTest>
{
    Task<IEnumerable<AssessmentEventTest>> GetByAssessmentEventAsync(Guid assessmentEventId, CancellationToken ct = default);
}
