using Microsoft.EntityFrameworkCore;
using Selecta.Core.Dtos;
using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class ReportTemplateRepository(SelectaDbContext context) : RepositoryBase<ReportTemplate, int>(context), IReportTemplateRepository
{
    protected override string DefaultOrderBy => "Name";

    private IQueryable<ReportTemplate> WithRelated() => DbSet.AsNoTracking()
        .Include(t => t.ProductionActivity)
        .Include(t => t.ReadingActivity)
        .Include(t => t.Header)
        .Include(t => t.Footer);

    public override async Task<ReportTemplate?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await WithRelated().FirstOrDefaultAsync(t => t.Id == id, ct);

    public override async Task<IEnumerable<ReportTemplate>> GetAllAsync(CancellationToken ct = default) =>
        await WithRelated().ToListAsync(ct);

    public override Task<PagedResult<ReportTemplate>> GetPagedAsync(
        int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default) =>
        PageAsync(WithRelated(), page, pageSize, filter, orderBy, ct);
}
