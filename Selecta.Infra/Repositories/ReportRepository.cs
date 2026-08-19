using Microsoft.EntityFrameworkCore;
using Selecta.Core.Dtos;
using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class ReportRepository(SelectaDbContext context) : RepositoryBase<Report>(context), IReportRepository
{
    protected override string DefaultOrderBy => "CreatedAt desc";

    private IQueryable<Report> WithRelated() => DbSet.AsNoTracking()
        .Include(r => r.AssessmentEvent!).ThenInclude(e => e!.Candidate)
        .Include(r => r.ReportTemplate)
        .Include(r => r.Responsible)
        .Include(r => r.Supervisor)
        .Include(r => r.ResponsibleSignature)
        .Include(r => r.SupervisorSignature);

    public override async Task<Report?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await WithRelated().FirstOrDefaultAsync(r => r.Id == id, ct);

    public override async Task<IEnumerable<Report>> GetAllAsync(CancellationToken ct = default) =>
        await WithRelated().ToListAsync(ct);

    public override Task<PagedResult<Report>> GetPagedAsync(
        int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default) =>
        PageAsync(WithRelated(), page, pageSize, filter, orderBy, ct);

    public async Task<Report?> GetByAssessmentEventAsync(Guid assessmentEventId, CancellationToken ct = default) =>
        await WithRelated().FirstOrDefaultAsync(r => r.AssessmentEventId == assessmentEventId, ct);
}
