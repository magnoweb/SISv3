using Selecta.Core.Entities.Common;

namespace Selecta.Core.Interfaces.Repositories;

public interface IContactRepository : IRepositoryBase<Contact>
{
    Task<IEnumerable<Contact>> GetByCompanyAsync(Guid companyId, CancellationToken ct = default);
}
