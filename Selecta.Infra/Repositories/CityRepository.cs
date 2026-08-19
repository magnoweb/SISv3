using Microsoft.EntityFrameworkCore;
using Selecta.Core.Entities.Common;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class CityRepository(SelectaDbContext context) : RepositoryBase<City>(context), ICityRepository
{
    protected override string DefaultOrderBy => "Name";

    public async Task<IEnumerable<City>> GetByStateAsync(string state, CancellationToken ct = default) =>
        await DbSet.AsNoTracking().Where(c => c.State == state).ToListAsync(ct);
}
