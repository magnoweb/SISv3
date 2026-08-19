using Selecta.Core.Entities.Selection;

namespace Selecta.Core.Interfaces.Repositories;

public interface ICompanyEvaluationResultRepository : IRepositoryBase<CompanyEvaluationResult>
{
    Task<IEnumerable<CompanyEvaluationResult>> GetByCompanyAsync(Guid companyId, CancellationToken ct = default);
}
