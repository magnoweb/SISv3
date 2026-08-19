using Microsoft.EntityFrameworkCore;
using Selecta.Core.Dtos;
using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class CompetencyScoreDescriptorRepository(SelectaDbContext context) : RepositoryBase<CompetencyScoreDescriptor>(context), ICompetencyScoreDescriptorRepository
{
    protected override string DefaultOrderBy => "CreatedAt desc";

    private IQueryable<CompetencyScoreDescriptor> WithRelated() => DbSet.AsNoTracking()
        .Include(d => d.CompetencyDescriptor)
        .Include(d => d.CompetencyScore);

    public override async Task<CompetencyScoreDescriptor?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await WithRelated().FirstOrDefaultAsync(d => d.Id == id, ct);

    public override async Task<IEnumerable<CompetencyScoreDescriptor>> GetAllAsync(CancellationToken ct = default) =>
        await WithRelated().ToListAsync(ct);

    public override Task<PagedResult<CompetencyScoreDescriptor>> GetPagedAsync(
        int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default) =>
        PageAsync(WithRelated(), page, pageSize, filter, orderBy, ct);
}
