using Microsoft.EntityFrameworkCore;
using Selecta.Core.Entities.Common;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class CompanyRepository(SelectaDbContext context) : RepositoryBase<Company>(context), ICompanyRepository
{
    protected override string DefaultOrderBy => "TradeName";

    public async Task<Company?> GetByDocumentAsync(string document, CancellationToken ct = default) =>
        await DbSet.FirstOrDefaultAsync(c => c.Document == document, ct);
}
