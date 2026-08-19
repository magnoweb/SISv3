using Microsoft.EntityFrameworkCore;
using Selecta.Core.Dtos;
using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class OpinionListEntryRepository(SelectaDbContext context) : RepositoryBase<OpinionListEntry>(context), IOpinionListEntryRepository
{
    private IQueryable<OpinionListEntry> WithRelated() => DbSet.AsNoTracking()
        .Include(e => e.AssessmentEvent!).ThenInclude(a => a!.Candidate)
        .Include(e => e.EvaluationResult);

    public override async Task<OpinionListEntry?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await WithRelated().FirstOrDefaultAsync(e => e.Id == id, ct);

    public override async Task<IEnumerable<OpinionListEntry>> GetAllAsync(CancellationToken ct = default) =>
        await WithRelated().ToListAsync(ct);

    public async Task<IEnumerable<OpinionListEntry>> GetByOpinionListAsync(Guid opinionListId, CancellationToken ct = default) =>
        await WithRelated().Where(e => e.OpinionListId == opinionListId).ToListAsync(ct);

    public override Task<PagedResult<OpinionListEntry>> GetPagedAsync(
        int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default) =>
        PageAsync(WithRelated(), page, pageSize, filter, orderBy, ct);
}
