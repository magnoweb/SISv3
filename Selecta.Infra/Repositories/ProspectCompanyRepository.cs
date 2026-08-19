using Microsoft.EntityFrameworkCore;
using Selecta.Core.Entities.Administrative;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class ProspectCompanyRepository(SelectaDbContext context) : RepositoryBase<ProspectCompany>(context), IProspectCompanyRepository
{
    protected override string DefaultOrderBy => "Name";

    public async Task<ProspectCompany?> GetByDocumentAsync(string document, CancellationToken ct = default) =>
        await DbSet.FirstOrDefaultAsync(p => p.Document == document, ct);
}
