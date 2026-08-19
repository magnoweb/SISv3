using Selecta.Core.Entities.Common;

namespace Selecta.Core.Interfaces.Repositories;

public interface IJobTitleRepository : IRepositoryBase<JobTitle>
{
    Task<IEnumerable<JobTitle>> GetByCompanyAsync(Guid companyId, CancellationToken ct = default);
}
