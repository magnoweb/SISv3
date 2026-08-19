using Microsoft.EntityFrameworkCore;
using Selecta.Core.Entities.Common;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class ContactRepository(SelectaDbContext context) : RepositoryBase<Contact>(context), IContactRepository
{
    protected override string DefaultOrderBy => "Name";

    public async Task<IEnumerable<Contact>> GetByCompanyAsync(Guid companyId, CancellationToken ct = default) =>
        await DbSet.AsNoTracking().Where(c => c.CompanyId == companyId).ToListAsync(ct);
}
