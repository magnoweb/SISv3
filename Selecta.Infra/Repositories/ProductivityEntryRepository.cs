using Microsoft.EntityFrameworkCore;
using Selecta.Core.Dtos;
using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class ProductivityEntryRepository(SelectaDbContext context) : RepositoryBase<ProductivityEntry>(context), IProductivityEntryRepository
{
    protected override string DefaultOrderBy => "Date desc";

    private IQueryable<ProductivityEntry> WithRelated() => DbSet.AsNoTracking()
        .Include(p => p.AssessmentEvent!).ThenInclude(e => e!.Candidate)
        .Include(p => p.Activity)
        .Include(p => p.User);

    public override async Task<ProductivityEntry?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await WithRelated().FirstOrDefaultAsync(p => p.Id == id, ct);

    public override async Task<IEnumerable<ProductivityEntry>> GetAllAsync(CancellationToken ct = default) =>
        await WithRelated().ToListAsync(ct);

    public async Task<IEnumerable<ProductivityEntry>> GetByAssessmentEventAsync(Guid assessmentEventId, CancellationToken ct = default) =>
        await WithRelated().Where(p => p.AssessmentEventId == assessmentEventId).ToListAsync(ct);

    public override Task<PagedResult<ProductivityEntry>> GetPagedAsync(
        int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default) =>
        PageAsync(WithRelated(), page, pageSize, filter, orderBy, ct);
}
