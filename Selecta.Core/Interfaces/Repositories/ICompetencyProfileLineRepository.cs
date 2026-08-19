using Selecta.Core.Entities.Selection;

namespace Selecta.Core.Interfaces.Repositories;

public interface ICompetencyProfileLineRepository : IRepositoryBase<CompetencyProfileLine>
{
    Task<IEnumerable<CompetencyProfileLine>> GetByProfileAsync(Guid competencyProfileId, CancellationToken ct = default);
}
