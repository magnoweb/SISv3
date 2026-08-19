using Microsoft.EntityFrameworkCore;
using Selecta.Core.Entities.Common;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class JobTitleRepository(SelectaDbContext context) : RepositoryBase<JobTitle>(context), IJobTitleRepository
{
    protected override string DefaultOrderBy => "Name";

    public async Task<IEnumerable<JobTitle>> GetByCompanyAsync(Guid companyId, CancellationToken ct = default) =>
        await DbSet.AsNoTracking().Where(j => j.CompanyId == companyId).ToListAsync(ct);
}
