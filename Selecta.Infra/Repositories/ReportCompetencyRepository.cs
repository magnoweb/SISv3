using Microsoft.EntityFrameworkCore;
using Selecta.Core.Dtos;
using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class ReportCompetencyRepository(SelectaDbContext context) : RepositoryBase<ReportCompetency>(context), IReportCompetencyRepository
{
    private IQueryable<ReportCompetency> WithRelated() => DbSet.AsNoTracking()
        .Include(c => c.Competency)
        .Include(c => c.CompetencyDescriptor)
        .Include(c => c.ProfileScore)
        .Include(c => c.Score);

    public override async Task<ReportCompetency?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await WithRelated().FirstOrDefaultAsync(c => c.Id == id, ct);

    public override async Task<IEnumerable<ReportCompetency>> GetAllAsync(CancellationToken ct = default) =>
        await WithRelated().ToListAsync(ct);

    public async Task<IEnumerable<ReportCompetency>> GetByReportAsync(Guid reportId, CancellationToken ct = default) =>
        await WithRelated().Where(c => c.ReportId == reportId).ToListAsync(ct);

    public override Task<PagedResult<ReportCompetency>> GetPagedAsync(
        int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default) =>
        PageAsync(WithRelated(), page, pageSize, filter, orderBy, ct);
}
