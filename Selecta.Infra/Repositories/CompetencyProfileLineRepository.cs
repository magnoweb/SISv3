using Microsoft.EntityFrameworkCore;
using Selecta.Core.Dtos;
using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class CompetencyProfileLineRepository(SelectaDbContext context) : RepositoryBase<CompetencyProfileLine>(context), ICompetencyProfileLineRepository
{
    private IQueryable<CompetencyProfileLine> WithRelated() => DbSet.AsNoTracking()
        .Include(l => l.CompetencyProfile)
        .Include(l => l.Competency)
        .Include(l => l.CompetencyScore);

    public override async Task<CompetencyProfileLine?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await WithRelated().FirstOrDefaultAsync(l => l.Id == id, ct);

    public override async Task<IEnumerable<CompetencyProfileLine>> GetAllAsync(CancellationToken ct = default) =>
        await WithRelated().ToListAsync(ct);

    public async Task<IEnumerable<CompetencyProfileLine>> GetByProfileAsync(Guid competencyProfileId, CancellationToken ct = default) =>
        await WithRelated().Where(l => l.CompetencyProfileId == competencyProfileId).ToListAsync(ct);

    public override Task<PagedResult<CompetencyProfileLine>> GetPagedAsync(
        int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default) =>
        PageAsync(WithRelated(), page, pageSize, filter, orderBy, ct);
}
