using Microsoft.EntityFrameworkCore;
using Selecta.Core.Dtos;
using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class JobTitleCompetencyProfileRepository(SelectaDbContext context) : RepositoryBase<JobTitleCompetencyProfile>(context), IJobTitleCompetencyProfileRepository
{
    protected override string DefaultOrderBy => "Name";

    private IQueryable<JobTitleCompetencyProfile> WithRelated() => DbSet.AsNoTracking().Include(p => p.JobTitle);

    public override async Task<JobTitleCompetencyProfile?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await WithRelated().FirstOrDefaultAsync(p => p.Id == id, ct);

    public override async Task<IEnumerable<JobTitleCompetencyProfile>> GetAllAsync(CancellationToken ct = default) =>
        await WithRelated().ToListAsync(ct);

    public override Task<PagedResult<JobTitleCompetencyProfile>> GetPagedAsync(
        int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default) =>
        PageAsync(WithRelated(), page, pageSize, filter, orderBy, ct);
}
