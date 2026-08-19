using Microsoft.EntityFrameworkCore;
using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class CompanyEvaluationResultRepository(SelectaDbContext context) : RepositoryBase<CompanyEvaluationResult>(context), ICompanyEvaluationResultRepository
{
    protected override string DefaultOrderBy => "Name";

    public async Task<IEnumerable<CompanyEvaluationResult>> GetByCompanyAsync(Guid companyId, CancellationToken ct = default) =>
        await DbSet.AsNoTracking().Where(r => r.CompanyId == companyId).ToListAsync(ct);
}
