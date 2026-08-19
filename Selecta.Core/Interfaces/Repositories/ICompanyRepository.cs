using Selecta.Core.Entities.Common;

namespace Selecta.Core.Interfaces.Repositories;

public interface ICompanyRepository : IRepositoryBase<Company>
{
    Task<Company?> GetByDocumentAsync(string document, CancellationToken ct = default);
}
