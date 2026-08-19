using Microsoft.EntityFrameworkCore;
using Selecta.Core.Dtos;
using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class ReportTemplateComponentRepository(SelectaDbContext context) : RepositoryBase<ReportTemplateComponent>(context), IReportTemplateComponentRepository
{
    private IQueryable<ReportTemplateComponent> WithRelated() => DbSet.AsNoTracking()
        .Include(c => c.ReportTemplate)
        .Include(c => c.ReportComponent);

    public override async Task<ReportTemplateComponent?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await WithRelated().FirstOrDefaultAsync(c => c.Id == id, ct);

    public override async Task<IEnumerable<ReportTemplateComponent>> GetAllAsync(CancellationToken ct = default) =>
        await WithRelated().ToListAsync(ct);

    public async Task<IEnumerable<ReportTemplateComponent>> GetByReportTemplateAsync(int reportTemplateId, CancellationToken ct = default) =>
        await WithRelated().Where(c => c.ReportTemplateId == reportTemplateId).ToListAsync(ct);

    public override Task<PagedResult<ReportTemplateComponent>> GetPagedAsync(
        int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default) =>
        PageAsync(WithRelated(), page, pageSize, filter, orderBy, ct);
}
