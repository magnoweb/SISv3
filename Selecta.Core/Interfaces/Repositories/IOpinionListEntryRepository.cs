using Selecta.Core.Entities.Selection;

namespace Selecta.Core.Interfaces.Repositories;

public interface IOpinionListEntryRepository : IRepositoryBase<OpinionListEntry>
{
    Task<IEnumerable<OpinionListEntry>> GetByOpinionListAsync(Guid opinionListId, CancellationToken ct = default);
}
