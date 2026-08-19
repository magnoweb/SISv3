using Selecta.Core.Entities.Administrative;

namespace Selecta.Core.Interfaces.Repositories;

public interface IProspectCompanyRepository : IRepositoryBase<ProspectCompany>
{
    Task<ProspectCompany?> GetByDocumentAsync(string document, CancellationToken ct = default);
}
